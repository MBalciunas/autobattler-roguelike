using System.Collections.Generic;
using System.Linq;
using AutoBattlerRoguelike.Scripts.Abilities;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class CoinTossProjectile : Area2D
{
    private float damage;
    private int poisonStacks;
    private int ricochetsRemaining;
    private float speed = 600f;
    private HashSet<Enemy> hitEnemies = new();

    public override void _Process(double delta)
    {
        Vector2 forward = Vector2.Right.Rotated(Rotation);
        GlobalPosition += forward * speed * (float)delta;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is Enemy enemy && !hitEnemies.Contains(enemy))
        {
            hitEnemies.Add(enemy);
            enemy.TakeDamage(damage);
            enemy.AddActiveDot(DamageOverTime.GetPoison(poisonStacks));

            if (ricochetsRemaining > 0)
            {
                var nextTarget = FindNextTarget();
                if (nextTarget != null)
                {
                    ricochetsRemaining--;
                    var direction = (nextTarget.GlobalPosition - GlobalPosition).Normalized();
                    Rotation = direction.Angle();
                    return;
                }
            }

            QueueFree();
        }
    }

    private Enemy FindNextTarget()
    {
        var enemies = GlobalManager.GetEnemiesSortedByClosest(GlobalPosition);
        return enemies.FirstOrDefault(e => !hitEnemies.Contains(e));
    }

    public void Init((float damage, int poisonStacks, int ricochets) stats)
    {
        damage = stats.damage;
        poisonStacks = stats.poisonStacks;
        ricochetsRemaining = stats.ricochets;
    }
}
