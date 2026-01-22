using System.Collections.Generic;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class ShadowStrike : Ability
{
    [Export] private PackedScene projectileScene;
    private float dashDistance = 200f;

    protected override async void ExecuteAbility()
    {
        List<Enemy> enemies = GlobalManager.GetEnemiesSortedByClosest();
        if (enemies.Count >= 1)
        {
            var enemy = enemies[0];
            var directionToEnemy = (enemy.GlobalPosition - GlobalPosition).Normalized();
            var dashDirection = -directionToEnemy;

            var tween = GetTree().CreateTween();
            var targetPos = GlobalManager.Player.GlobalPosition + dashDirection * dashDistance;
            tween.TweenProperty(GlobalManager.Player, "global_position", targetPos, 0.15);
            await ToSignal(tween, Tween.SignalName.Finished);

            var stats = GetStats();
            var projectile = projectileScene.Instantiate<ShadowStrikeProjectile>();
            projectile.Init((stats.damage, stats.slow, stats.slowDuration));
            projectile.GlobalPosition = GlobalPosition;
            projectile.Rotation = directionToEnemy.Angle();
            GetTree().Root.GetNode("MainLevel").AddChild(projectile);
        }
    }
}