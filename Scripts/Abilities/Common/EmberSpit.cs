using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class EmberSpit : Ability
{
    [Export] private PackedScene projectileScene;

    protected override void ExecuteAbility()
    {
        var enemy = GlobalManager.GetEnemiesSortedByClosest().FirstOrDefault();

        if (enemy != null)
        {
            var stats = GetStats();
            var projectile = projectileScene.Instantiate<EmberSpitProjectile>();
            projectile.Init((stats.damage, stats.burnStacks, stats.explosionRadius));
            projectile.GlobalPosition = GlobalPosition;
            var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();
            projectile.Rotation = direction.Angle();
            GetTree().Root.GetNode("MainLevel").AddChild(projectile);
        }
    }
}
