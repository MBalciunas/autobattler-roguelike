using System;
using System.Collections.Generic;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Rare;

public partial class CrimsonSpike : Ability
{
    [Export] private PackedScene projectileScene;

    protected override void ExecuteAbility()
    {
        List<Enemy> enemies = GlobalManager.GetEnemiesSortedByClosest();

        if (enemies.Count >= 1)
        {
            var enemy = enemies[^1];

            var crimsonSpike = projectileScene.Instantiate<CrimsonSpikeProjectile>();
            crimsonSpike.Init(GetStatsForLevel(Level));
            crimsonSpike.GlobalPosition = GlobalPosition;
            var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();
            crimsonSpike.Rotation = direction.Angle();
            GetTree().Root.GetNode("MainLevel").AddChild(crimsonSpike);
        }
    }

    private int GetStatsForLevel(int level)
    {
        return level switch
        {
            1 => 2,
            2 => 4,
            3 => 8,
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }
}