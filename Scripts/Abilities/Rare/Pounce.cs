using System.Collections.Generic;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Uncommon;

public partial class Pounce : Ability
{
    protected override async void ExecuteAbility()
    {
        List<Enemy> enemies = GlobalManager.GetEnemiesSortedByClosest();
        if (enemies.Count >= 1)
        {
            var enemy = enemies[0];
            var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();

            var tween = GetTree().CreateTween();
            var targetPos = enemy.GlobalPosition - direction * 30;
            tween.TweenProperty(GlobalManager.Player, "global_position", targetPos, 0.1);
            await ToSignal(tween, Tween.SignalName.Finished);

            var damage = GetStatsForLevel(Level);
            enemy.TakeDamage(damage);
            GlobalManager.playerState.AddShield(damage / 5);
        }
    }

    private float GetStatsForLevel(int level)
    {
        // Match Data/Abilities.json: damage 10/20/50; shield gained equals damage dealt
        return level switch
        {
            1 => 10f,
            2 => 20f,
            3 => 50f,
            _ => 10f
        };
    }
}