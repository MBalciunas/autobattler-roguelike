using System.Collections.Generic;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class ShadowStrike : Ability
{
    protected override void ExecuteAbility()
    {
        List<Enemy> enemies = GlobalManager.GetEnemiesSortedByClosest();
        if (enemies.Count >= 1)
        {
            var enemy = enemies[^1];
            var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();

            // Teleport behind the furthest enemy (opposite of approach direction)
            GlobalManager.Player.GlobalPosition = enemy.GlobalPosition - direction * 120;
            enemy.TakeDamage(GetStats().damage);
        }
    }
}