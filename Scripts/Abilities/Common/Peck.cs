using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class Peck : Ability
{
    [Export] private PackedScene projectileScene;

    protected override void ExecuteAbility()
    {
        var enemies = GlobalManager.GetEnemiesSortedByClosest();
        if (enemies.Count == 0) return;

        var stats = GetStats();
        int projectileCount = stats.projectileCount > 0 ? stats.projectileCount : 1;

        // Target closest enemies for each projectile
        var targets = enemies.Take(projectileCount).ToList();

        // If not enough targets, cycle through available enemies
        while (targets.Count < projectileCount && enemies.Count > 0)
        {
            targets.Add(enemies[targets.Count % enemies.Count]);
        }

        for (int i = 0; i < targets.Count; i++)
        {
            var target = targets[i];
            var projectile = projectileScene.Instantiate<PeckProjectile>();
            projectile.Init((stats.damage, stats.shield));
            projectile.GlobalPosition = GlobalPosition;

            var direction = (target.GlobalPosition - GlobalPosition).Normalized();
            // Slight spread for multiple projectiles
            if (targets.Count > 1)
            {
                float spreadAngle = (i - (targets.Count - 1) / 2f) * 0.15f;
                direction = direction.Rotated(spreadAngle);
            }
            projectile.Rotation = direction.Angle();

            GetTree().Root.GetNode("MainLevel").AddChild(projectile);
        }
    }
}
