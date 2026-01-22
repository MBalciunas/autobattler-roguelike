using System.Collections.Generic;
using AutoBattlerRoguelike.Scripts.UI;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class QuickJabEffect : Area2D
{
    private float damage;
    private float goldOnKillChance;
    private HashSet<Enemy> hitEnemies = new();

    private void OnAreaEntered(Area2D area)
    {
        if (area is Enemy enemy && !hitEnemies.Contains(enemy))
        {
            hitEnemies.Add(enemy);

            if (goldOnKillChance > 0)
            {
                var enemyPosition = enemy.GlobalPosition;
                enemy.Died += () => OnEnemyKilled(enemyPosition);
            }

            enemy.TakeDamage(damage);
        }
    }

    private void OnEnemyKilled(Vector2 position)
    {
        if (GD.Randf() < goldOnKillChance)
        {
            GlobalManager.playerState.Gold.Add(1);
            FloatingText.SpawnGold(GetTree().Root.GetNode("MainLevel"), position, 1);
        }
    }

    public void Init((float damage, float goldOnKillChance) stats)
    {
        damage = stats.damage;
        goldOnKillChance = stats.goldOnKillChance;
    }
}