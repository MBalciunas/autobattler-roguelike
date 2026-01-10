using System;
using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Uncommon;

public partial class VenomFang : Ability
{
    [Export] private PackedScene venomFangEffectScene;
    private float range = 230f;
    private Tween tween;

    public override void _Ready() { }

    protected override void ExecuteAbility()
    {
        var closest3Enemies = GlobalManager.GetEnemiesSortedByClosest().Take(3).ToList();

        var stats = GetStatsForLevel(Level);
        foreach (var enemy in closest3Enemies)
        {
            var effect = venomFangEffectScene.Instantiate<VenomFangEffect>();
            var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();
            effect.GlobalPosition = GlobalPosition + direction * 20;
            effect.Rotation = direction.Angle();
            if (enemy.GlobalPosition.DistanceTo(GlobalPosition) <= range)
            {
                GetTree().Root.GetNode("MainLevel").AddChild(effect);
                enemy.TakeDamage(stats.damage);
                enemy.AddActiveDot(new DamageOverTime(stats.bleedDamage, stats.bleedDuration, ElementType.Bleed));
                enemy.AddActiveDot(new DamageOverTime(stats.poisonDamage, stats.poisonDuration, ElementType.Poison));
            }
        }
    }

    private (float damage, float bleedDamage, float bleedDuration, float poisonDamage, float poisonDuration)
        GetStatsForLevel(int level)
    {
        // Match Data/Abilities.json: damage 3/9/25; bleed DPS 4/10/18 for 4s; poison DPS 2/5/12 for 10s
        return level switch
        {
            1 => (damage: 3f, bleedDamage: 4f, bleedDuration: 4, poisonDamage: 2f, poisonDuration: 10f),
            2 => (damage: 9f, bleedDamage: 10f, bleedDuration: 4, poisonDamage: 5f, poisonDuration: 10f),
            3 => (damage: 25f, bleedDamage: 18f, bleedDuration: 4, poisonDamage: 12f, poisonDuration: 10f),
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }
}