using System;
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
            var toxicDart = projectileScene.Instantiate<SteelPlumeProjectile>();
            toxicDart.Init(GetStatsForLevel(Level));
            toxicDart.GlobalPosition = GlobalPosition;
            var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();
            toxicDart.Rotation = direction.Angle();
            GetTree().Root.GetNode("MainLevel").AddChild(toxicDart);
        }
    }

    private (float damage, float slow, int slowDuration) GetStatsForLevel(int level)
    {
        return level switch
        {
            1 => (damage: 20f, slow: 0.3f, slowDuration: 4),
            2 => (damage: 40f, slow: 0.4f, slowDuration: 5),
            3 => (damage: 80f, slow: 0.5f, slowDuration: 6),
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }
}