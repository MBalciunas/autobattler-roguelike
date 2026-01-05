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
            GlobalManager.playerState.Shield.Add(damage);
        }
    }

    private float GetStatsForLevel(int level)
    {
        return level switch
        {
            1 => 60f,
            2 => 120f,
            3 => 200f,
            _ => 1.0f
        };
    }
}