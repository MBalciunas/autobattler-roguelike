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

            var stats = GetStats();
            var crimsonSpike = projectileScene.Instantiate<CrimsonSpikeProjectile>();
            crimsonSpike.Init(stats.bleedStacks);
            crimsonSpike.GlobalPosition = GlobalPosition;
            var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();
            crimsonSpike.Rotation = direction.Angle();
            GetTree().Root.GetNode("MainLevel").AddChild(crimsonSpike);
        }
    }
}