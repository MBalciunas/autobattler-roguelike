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

        int dartCount = Level switch
        {
            1 => 1,
            2 => 2,
            3 => 3,
            _ => 1
        };

        var targets = enemies.Take(dartCount).ToList();
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

    private (float damage, int poisonStacks) GetStatsForLevel(int level)
    {
        return level switch
        {
            1 => (damage: 5f, poisonStacks: 1),
            2 => (damage: 9f, poisonStacks: 2),
            3 => (damage: 15f, poisonStacks: 3),
        };
    }
}