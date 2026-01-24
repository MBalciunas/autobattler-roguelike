using System.Collections.Generic;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class CoinToss : Ability
{
    [Export] private PackedScene projectileScene;

    protected override void ExecuteAbility()
    {
        var enemies = GlobalManager.GetEnemiesSortedByClosest();
        if (enemies.Count == 0) return;

        // Pick a random enemy
        var randomIndex = GD.RandRange(0, enemies.Count - 1);
        var target = enemies[randomIndex];

        var stats = GetStats();
        var projectile = projectileScene.Instantiate<CoinTossProjectile>();
        projectile.Init((stats.damage, stats.poisonStacks, stats.ricochets));
        projectile.GlobalPosition = GlobalPosition;

        var direction = (target.GlobalPosition - GlobalPosition).Normalized();
        projectile.Rotation = direction.Angle();

        GetTree().Root.GetNode("MainLevel").AddChild(projectile);
    }
}
