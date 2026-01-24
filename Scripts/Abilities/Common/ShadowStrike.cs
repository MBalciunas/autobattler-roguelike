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

            var targetPos = GlobalManager.Player.GlobalPosition + dashDirection * dashDistance;
            targetPos = ClampToBounds(targetPos);

            var tween = GetTree().CreateTween();
            tween.TweenProperty(GlobalManager.Player, "global_position", targetPos, 0.15);
            await ToSignal(tween, Tween.SignalName.Finished);

            var stats = GetStats();
            directionToEnemy = (enemy.GlobalPosition - GlobalPosition).Normalized();

            var projectile = projectileScene.Instantiate<ShadowStrikeProjectile>();
            projectile.Init((stats.damage, stats.slow, stats.slowDuration));
            projectile.GlobalPosition = GlobalPosition;
            projectile.Rotation = directionToEnemy.Angle();
            GetTree().Root.GetNode("MainLevel").AddChild(projectile);
        }
    }

    private Vector2 ClampToBounds(Vector2 position)
    {
        var screenRect = GetViewport().GetVisibleRect();
        float bottomUiHeight = 150f;
        float buffer = 15f;

        var minX = screenRect.Position.X + buffer;
        var maxX = screenRect.End.X - buffer;
        var minY = screenRect.Position.Y + buffer;
        var maxY = screenRect.End.Y - buffer - bottomUiHeight;

        return new Vector2(
            Mathf.Clamp(position.X, minX, maxX),
            Mathf.Clamp(position.Y, minY, maxY)
        );
    }
}