using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Rare;

public partial class ToxicPool : Ability
{
    [Export] private PackedScene effectScene;

    protected override void ExecuteAbility()
    {
        var enemy = GlobalManager.GetEnemiesSortedByClosest().FirstOrDefault();

        if (enemy != null)
        {
            var toxicPool = effectScene.Instantiate<ToxicPoolEffect>();
            toxicPool.Init(GetStatsForLevel(Level));
            toxicPool.GlobalPosition = enemy.Position;
            GetTree().Root.GetNode("MainLevel").AddChild(toxicPool);
        }
    }

    private (float damage, float poisonDamage, float poisonDuration, float slow, float duration) GetStatsForLevel(int level)
    {
        return level switch
        {
            1 => (damage: 4f, poisonDamage: 4f, poisonDuration: 10f, slow: 0.35f, duration: 3f),
            2 => (damage: 10f, poisonDamage: 7f, poisonDuration: 10f, slow: 0.45f, duration: 3f),
            3 => (damage: 20f, poisonDamage: 12f, poisonDuration: 10f, slow: 0.6f, duration: 3f),
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }
}