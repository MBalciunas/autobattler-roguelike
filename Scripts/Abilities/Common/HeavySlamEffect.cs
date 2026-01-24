using System.Collections.Generic;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class HeavySlamEffect : Area2D
{
    private float damage;
    private float knockbackStrength;
    private float range;
    private Vector2 slamDirection;
    private HashSet<Enemy> hitEnemies = new();
    private CollisionShape2D collisionShape;

    public override void _Ready()
    {
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        if (collisionShape.Shape is CapsuleShape2D capsule)
        {
            capsule.Height = range;
        }

        // Quick animation then destroy
        var tween = CreateTween();
        tween.TweenInterval(0.15f);
        tween.TweenCallback(Callable.From(QueueFree));
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is Enemy enemy && !hitEnemies.Contains(enemy))
        {
            hitEnemies.Add(enemy);
            enemy.TakeDamage(damage);

            // Knock to the side (perpendicular to slam direction)
            var toEnemy = (enemy.GlobalPosition - GlobalPosition).Normalized();
            var perpendicular = new Vector2(-slamDirection.Y, slamDirection.X);

            // Determine which side the enemy is on
            float side = toEnemy.Dot(perpendicular);
            var knockbackDir = side >= 0 ? perpendicular : -perpendicular;

            enemy.Knockback(knockbackStrength, knockbackDir);
        }
    }

    public void Init((float damage, float knockbackStrength, float range, Vector2 direction) stats)
    {
        damage = stats.damage;
        knockbackStrength = stats.knockbackStrength;
        range = stats.range;
        slamDirection = stats.direction;
    }
}
