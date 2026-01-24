using System.Collections.Generic;
using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Uncommon;

public partial class Pounce : Ability
{
    private const float JumpDuration = 0.1f;
    private const float JumpAwayDistance = 200f;

    protected override async void ExecuteAbility()
    {
        var enemies = GlobalManager.GetEnemiesSortedByClosest();
        if (enemies.Count == 0) return;

        var stats = GetStats();
        var hitEnemies = new HashSet<Enemy>();
        Enemy lastEnemy = null;

        GlobalManager.Player.isInvulnerable = true;

        // Ricochet through enemies
        for (int i = 0; i < stats.ricochets + 1; i++)
        {
            // Find next closest enemy that hasn't been hit yet and is still valid
            var nextEnemy = enemies.FirstOrDefault(e => IsInstanceValid(e) && !hitEnemies.Contains(e));
            if (nextEnemy == null) break;

            // Jump to the enemy
            var tween = GetTree().CreateTween();
            tween.TweenProperty(GlobalManager.Player, "global_position", nextEnemy.GlobalPosition, JumpDuration);
            await ToSignal(tween, Tween.SignalName.Finished);

            // Deal damage and apply bleed
            nextEnemy.TakeDamage(stats.damage);
            nextEnemy.AddActiveDot(DamageOverTime.GetBleed(stats.bleedStacks));

            hitEnemies.Add(nextEnemy);
            lastEnemy = nextEnemy;

            // Re-sort enemies from new position for next bounce, filtering out dead enemies
            enemies = GlobalManager.GetEnemiesSortedByClosest()
                .Where(e => IsInstanceValid(e))
                .ToList();
        }

        // Jump away from last enemy in direction with no enemies
        if (lastEnemy != null)
        {
            await JumpAwayFromEnemy(lastEnemy);
        }

        GlobalManager.Player.isInvulnerable = false;
        GD.Print("Finished Pounce");
    }

    private async System.Threading.Tasks.Task JumpAwayFromEnemy(Enemy lastEnemy)
    {
        var enemies = GlobalManager.GetEnemiesSortedByClosest();
        var playerPos = GlobalManager.Player.GlobalPosition;

        // Direction away from the last enemy hit
        var awayFromLast = (playerPos - lastEnemy.GlobalPosition).Normalized();

        // Find best direction (away from enemies)
        Vector2 bestDirection = awayFromLast;
        float bestScore = float.MinValue;

        // Test 8 directions and pick the one furthest from all enemies
        for (int i = 0; i < 8; i++)
        {
            float angle = i * Mathf.Pi / 4f;
            var testDir = Vector2.Right.Rotated(angle);
            var testPos = playerPos + testDir * JumpAwayDistance;

            // Score based on distance to nearest enemy at that position
            float minDistToEnemy = float.MaxValue;
            foreach (var enemy in enemies)
            {
                float dist = testPos.DistanceTo(enemy.GlobalPosition);
                if (dist < minDistToEnemy)
                    minDistToEnemy = dist;
            }

            // Prefer directions away from last enemy
            float awayBonus = testDir.Dot(awayFromLast) * 50f;
            float score = minDistToEnemy + awayBonus;

            if (score > bestScore)
            {
                bestScore = score;
                bestDirection = testDir;
            }
        }

        var safePosition = playerPos + bestDirection * JumpAwayDistance;

        var tween = GetTree().CreateTween();
        tween.TweenProperty(GlobalManager.Player, "global_position", safePosition, JumpDuration);
        await ToSignal(tween, Tween.SignalName.Finished);
    }
}
