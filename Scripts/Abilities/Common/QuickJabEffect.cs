using System.Collections.Generic;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class QuickJabEffect : Area2D
{
    private float damage;
    private HashSet<Enemy> hitEnemies = new();

    private void OnAreaEntered(Area2D area)
    {
        if (area is Enemy enemy && !hitEnemies.Contains(enemy))
        {
            hitEnemies.Add(enemy);
            enemy.TakeDamage(damage);
        }
    }

    public void Init((float damage, float width) stats)
    {
        damage = stats.damage;
    }
}
