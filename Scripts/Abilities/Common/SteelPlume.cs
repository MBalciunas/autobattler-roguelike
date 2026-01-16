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

    private (float damage, float slow, float slowDuration) GetStatsForLevel(int level)
    {
        // Match Data/Abilities.json: damage 2/7/15; slow 20%/30%/40%; duration 3/4/5s
        return level switch
        {
            1 => (damage: 3f, slow: 0.20f, slowDuration: 3f),
            2 => (damage: 7f, slow: 0.30f, slowDuration: 4f),
            3 => (damage: 15f, slow: 0.40f, slowDuration: 5f),
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }
}