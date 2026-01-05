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

    private (float bleedDamage, float bleedDuration) GetStatsForLevel(int level)
    {
        return level switch
        {
            1 => (bleedDamage: 10f, bleedDuration: 4),
            2 => (bleedDamage: 20f, bleedDuration: 4),
            3 => (bleedDamage: 40f, bleedDuration: 4),
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }
}