using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class SteelPlume : Ability
{
    [Export] private PackedScene projectileScene;

    protected override void ExecuteAbility()
    {
        var enemy = GlobalManager.GetEnemiesSortedByClosest().FirstOrDefault();

        if (enemy != null)
        {
            var stats = GetStats();
            var projectile = projectileScene.Instantiate<SteelPlumeProjectile>();
            projectile.Init((stats.damage, stats.slow, stats.slowDuration));
            projectile.GlobalPosition = GlobalPosition;
            var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();
            projectile.Rotation = direction.Angle();
            GetTree().Root.GetNode("MainLevel").AddChild(projectile);
        }
    }
}