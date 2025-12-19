using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class ToxicDart : Ability
{
    [Export] private PackedScene toxicDartProjectileScene;

    protected override void ExecuteAbility()
    {
        var enemy = GlobalManager.GetEnemiesSortedByClosest().FirstOrDefault();

        if (enemy != null)
        {
            var toxicDart = toxicDartProjectileScene.Instantiate<ToxicDartProjectile>();
            toxicDart.Init(GetStatsForLevel(Level));
            toxicDart.GlobalPosition = GlobalPosition;
            var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();
            toxicDart.Rotation = direction.Angle();
            GetTree().Root.GetNode("MainLevel").AddChild(toxicDart);
        }
    }

    private (float cleaveDamage, float bleedDamage, int bleedDuration) GetStatsForLevel(int level)
    {
        return level switch
        {
            1 => (cleaveDamage: 1.0f, bleedDamage: 0.2f, bleedDuration: 4),
            2 => (cleaveDamage: 1.8f, bleedDamage: 0.4f, bleedDuration: 5),
            3 => (cleaveDamage: 3.24f, bleedDamage: 0.75f, bleedDuration: 6),
            _ => (cleaveDamage: 1.0f, bleedDamage: 0.2f, bleedDuration: 4)
        };
    }
}