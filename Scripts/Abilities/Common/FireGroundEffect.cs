using System.Collections.Generic;
using AutoBattlerRoguelike.Scripts.Abilities;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class FireGroundEffect : Area2D
{
    private float damage;
    private float duration;
    private Timer tickTimer;
    private Timer durationTimer;
    private HashSet<Enemy> enemiesInArea = new();

    public override void _Ready()
    {
        tickTimer = new Timer();
        tickTimer.WaitTime = 0.5f;
        tickTimer.OneShot = false;
        tickTimer.Timeout += OnTick;
        AddChild(tickTimer);
        tickTimer.Start();

        durationTimer = new Timer();
        durationTimer.WaitTime = duration;
        durationTimer.OneShot = true;
        durationTimer.Timeout += () => QueueFree();
        AddChild(durationTimer);
        durationTimer.Start();
    }

    private void OnTick()
    {
        foreach (var enemy in enemiesInArea)
        {
            if (IsInstanceValid(enemy))
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is Enemy enemy)
        {
            enemiesInArea.Add(enemy);
            enemy.TakeDamage(damage);
        }
    }

    private void OnAreaExited(Area2D area)
    {
        if (area is Enemy enemy)
        {
            enemiesInArea.Remove(enemy);
        }
    }

    public void Init((float damage, float duration) stats)
    {
        damage = stats.damage;
        duration = stats.duration;
    }
}
