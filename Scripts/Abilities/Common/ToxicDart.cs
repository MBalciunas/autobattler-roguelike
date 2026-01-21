using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class ToxicDart : Ability
{
    [Export] private PackedScene toxicDartProjectileScene;

    protected override void ExecuteAbility()
    {
        var enemies = GlobalManager.GetEnemiesSortedByClosest();
        if (enemies.Count == 0) return;

        var stats = GetStats();
        int dartCount = stats.projectileCount > 0 ? stats.projectileCount : 1;

        var targets = enemies.Take(dartCount).ToList();
        if (targets.Count < dartCount)
        {
            var additional = enemies.Except(targets).Take(dartCount - targets.Count);
            targets.AddRange(additional);
        }

        foreach (var target in targets)
        {
            var toxicDart = toxicDartProjectileScene.Instantiate<ToxicDartProjectile>();
            toxicDart.Init((stats.damage, stats.poisonStacks));
            toxicDart.GlobalPosition = GlobalPosition;
            var direction = (target.GlobalPosition - GlobalPosition).Normalized();
            toxicDart.Rotation = direction.Angle();
            GetTree().Root.GetNode("MainLevel").AddChild(toxicDart);
        }
    }
}