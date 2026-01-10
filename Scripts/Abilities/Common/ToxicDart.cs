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

        // JSON: level1=1 dart, level2=2 darts, level3=3 darts. Shoot closest unpoisoned enemies first.
        int dartCount = Level switch
        {
            1 => 1,
            2 => 2,
            3 => 3,
            _ => 1
        };

        // Prefer unpoisoned enemies, then fill with remaining closest if not enough
        var targets = enemies.Where(e => !e.IsPoisoned()).Take(dartCount).ToList();
        if (targets.Count < dartCount)
        {
            var additional = enemies.Except(targets).Take(dartCount - targets.Count);
            targets.AddRange(additional);
        }

        foreach (var target in targets)
        {
            var toxicDart = toxicDartProjectileScene.Instantiate<ToxicDartProjectile>();
            toxicDart.Init(GetStatsForLevel(Level));
            toxicDart.GlobalPosition = GlobalPosition;
            var direction = (target.GlobalPosition - GlobalPosition).Normalized();
            toxicDart.Rotation = direction.Angle();
            GetTree().Root.GetNode("MainLevel").AddChild(toxicDart);
        }
    }

    private (float damage, float poisonDamage, int poisonDuration) GetStatsForLevel(int level)
    {
        // Updated to match Data/Abilities.json for Toxic Dart (damage + poison DPS for 10s)
        return level switch
        {
            1 => (damage: 3f, poisonDamage: 2f, poisonDuration: 10),
            2 => (damage: 6f, poisonDamage: 4f, poisonDuration: 10),
            3 => (damage: 15f, poisonDamage: 8f, poisonDuration: 10),
            _ => (damage: 3f, poisonDamage: 2f, poisonDuration: 10)
        };
    }
}