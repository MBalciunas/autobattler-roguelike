using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class ToxicDart : Ability
{
    [Export] private PackedScene toxicDartProjectileScene;

    public override void Execute()
    {
        ExecuteToxicDart();
    }

    private async void ExecuteToxicDart()
    {
        await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);

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

        EmitSignal(Ability.SignalName.Finished, this);
    }
    
    private (float projectileDamage, float poisonDamage, int poisonDuration) GetStatsForLevel(int level)
    {
        return level switch
        {
            1 => (projectileDamage: 1.0f, poisonDamage: 0.2f, poisonDuration: 4),
            2 => (projectileDamage: 1.8f, poisonDamage: 0.4f, poisonDuration: 5),
            3 => (projectileDamage: 3.24f, poisonDamage: 0.75f, poisonDuration: 6),
            _ => (projectileDamage: 1.0f, poisonDamage: 0.2f, poisonDuration: 4)
        };
    }
}