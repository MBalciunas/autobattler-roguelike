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
            //TODO shoot toxic darts amount based on level 
            toxicDart.Init(GetStatsForLevel(Level));
            toxicDart.GlobalPosition = GlobalPosition;
            var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();
            toxicDart.Rotation = direction.Angle();
            GetTree().Root.GetNode("MainLevel").AddChild(toxicDart);
        }
    }

    private (float damage, float poisonDamage, int poisonDuration) GetStatsForLevel(int level)
    {
        return level switch
        {
            1 => (damage: 1.0f, poisonDamage: 0.5f, poisonDuration: 4),
            2 => (damage: 1.8f, poisonDamage: 0.1f, poisonDuration: 5),
            3 => (damage: 3.24f, poisonDamage: 0.2f, poisonDuration: 6),
            _ => (damage: 1.0f, poisonDamage: 0.2f, poisonDuration: 4)
        };
    }
}