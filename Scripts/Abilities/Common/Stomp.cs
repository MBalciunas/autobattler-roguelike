using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class Stomp : Ability
{
    [Export] private PackedScene stompEffectScene;
    private Sprite2D stompEffect;

    public override void _Ready()
    {
        stompEffect = GetNode<Sprite2D>("Sprite");
        stompEffect.Modulate =  new Color(0, 0, 0, 0);

    }

    protected override void ExecuteAbility()
    {
        var (stompDamage, knockbackStrength, knockbackRadius) = GetStatsForLevel(Level);
        // stompEffectScene.Instantiate();
        stompEffect.Modulate = new Color(1, 1, 1);
        stompEffect.Scale = Vector2.One * knockbackRadius / 128f;
        var tween = GetTree().CreateTween();
        Color endColor = new(0, 0, 0, 0);
        tween.TweenProperty(stompEffect, "modulate", endColor, 1f);
        GlobalManager.GetEnemiesSortedByClosest()
            .Where(e => e.GlobalPosition.DistanceTo(GlobalPosition) < knockbackRadius)
            .ToList().ForEach(e =>
            {
                var direction = GlobalPosition.DirectionTo(e.GlobalPosition).Normalized();
                e.TakeDamage(stompDamage);
                e.Knockback(knockbackStrength, direction);
            });
    }

    private (float stompDamage, float knockbackStrength, float knockbackRadius) GetStatsForLevel(int level)
    {
        return level switch
        {
            1 => (stompDamage: 0.5f, knockbackStrength: 100f, knockbackRadius: 300f),
            2 => (stompDamage: 1f, knockbackStrength: 150f, knockbackRadius: 400f),
            3 => (stompDamage: 2f, knockbackStrength: 200f, knockbackRadius: 500f),
            _ => (stompDamage: 0.5f, knockbackStrength: 0.2f, knockbackRadius: 300f)
        };
    }
}