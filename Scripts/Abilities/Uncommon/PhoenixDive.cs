using System.Collections.Generic;
using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Uncommon;

public partial class PhoenixDive : Ability
{
    [Export] private PackedScene landEffectScene;
    private Sprite2D landEffect;

    public override void _Ready()
    {
        landEffect = GetNode<Sprite2D>("Sprite");
        landEffect.Modulate = new Color(0, 0, 0, 0);
    }

    protected override async void ExecuteAbility()
    {
        List<Enemy> enemies = GlobalManager.GetEnemiesSortedByClosest();
        if (enemies.Count >= 1)
        {
            var enemy = enemies[^1];

            var tween = GetTree().CreateTween();
            GlobalManager.Player.isInvulnerable = true;
            tween.TweenProperty(GlobalManager.Player, "global_position", enemy.GlobalPosition, 0.4);
            await ToSignal(tween, Tween.SignalName.Finished);

            LandEffect();
        }
    }

    private void LandEffect()
    {
        var stats = GetStats();
        landEffect.Modulate = Color.Color8(255, 100, 255);
        landEffect.Scale = Vector2.One * stats.knockbackRadius / 128f;
        var landEffectTween = GetTree().CreateTween();
        Color endColor = new(0, 0, 0, 0);
        landEffectTween.TweenProperty(landEffect, "modulate", endColor, 1f);
        GlobalManager.GetEnemiesSortedByClosest()
            .Where(e => e.GlobalPosition.DistanceTo(GlobalPosition) < stats.knockbackRadius)
            .ToList().ForEach(e =>
            {
                var direction = GlobalPosition.DirectionTo(e.GlobalPosition).Normalized();
                e.TakeDamage(stats.damage);
                e.Knockback(stats.knockbackStrength, direction);
            });

        GlobalManager.Player.isInvulnerable = false;
    }
}