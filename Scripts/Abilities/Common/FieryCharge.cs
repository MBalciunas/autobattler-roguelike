using System;
using System.Linq;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class FieryCharge : Ability
{
    public override void _Ready() { }

    protected override void ExecuteAbility()
    {
        var enemy = GlobalManager.GetEnemiesSortedByClosest().FirstOrDefault();

        if (enemy != null)
        {
            var (fireDamage, fireDuration, chargeDistance) = GetStatsForLevel(Level);
            var direction = GlobalPosition.DirectionTo(enemy.GlobalPosition);
            var dot = new DamageOverTime(fireDamage, fireDuration, DamageType.Fire);
            GlobalManager.Player.StartCharging(direction, chargeDistance, 0, 0, dot);
        }
    }

    private (float fireDamage, float fireDuration, int chargeDistance) GetStatsForLevel(int level)
    {
        return level switch
        {
            1 => (fireDamage: 0.4f, fireDuration: 4f, chargeDistance: 500),
            2 => (fireDamage: 0.9f, fireDuration: 5f, chargeDistance: 700),
            3 => (fireDamage: 1.5f, fireDuration: 6f, chargeDistance: 900),
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }
}