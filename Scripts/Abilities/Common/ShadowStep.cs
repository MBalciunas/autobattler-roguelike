using System.Collections.Generic;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class ShadowStep : Ability
{
    public override void Execute()
    {
        ExecuteToxicDart();
    }

    private async void ExecuteToxicDart()
    {
        await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);

        List<Enemy> enemies = GlobalManager.GetEnemiesSortedByClosest();
        if (enemies.Count >= 1)
        {
            var enemy = enemies[^1];
        
            var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();

            GlobalManager.Player.GlobalPosition = enemy.GlobalPosition + direction * 120;
            enemy.TakeDamage(GetDamageForLevel(Level));
        }

        EmitSignal(Ability.SignalName.Finished, this);
    }
    
    private float GetDamageForLevel(int level)
    {
        return level switch
        {
            1 =>  3f,
            2 => 4.5f,
            3 => 6f,
            _ => 3f
        };
    }
}