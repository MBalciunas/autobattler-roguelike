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
        stompEffect.Modulate = new Color(0, 0, 0, 0);
    }

    protected override void ExecuteAbility()
    {
        var stats = GetStats();
        stompEffect.Modulate = new Color(1, 1, 1);
        stompEffect.Scale = Vector2.One * stats.knockbackRadius / 128f;
        var tween = GetTree().CreateTween();
        Color endColor = new(0, 0, 0, 0);
        tween.TweenProperty(stompEffect, "modulate", endColor, 1f);
        GlobalManager.GetEnemiesSortedByClosest()
            .Where(e => e.GlobalPosition.DistanceTo(GlobalPosition) < stats.knockbackRadius)
            .ToList().ForEach(e =>
            {
                var direction = GlobalPosition.DirectionTo(e.GlobalPosition).Normalized();
                e.Knockback(stats.knockbackStrength, direction);
                e.TakeDamage(stats.damage);
            });
    }
}