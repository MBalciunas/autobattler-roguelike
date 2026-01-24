using System.Collections.Generic;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Uncommon;

public partial class IronFangWall : Area2D
{
    private float damage;
    private Vector2 targetPosition;
    private Vector2 pushDirection;
    private float speed = 400f;
    private float pushStrength = 600f;
    private bool reachedTarget;
    private readonly HashSet<Enemy> enemiesHit = new();
    private readonly HashSet<Enemy> enemiesPushed = new();

    public override void _Process(double delta)
    {
        if (reachedTarget) return;

        Vector2 forward = Vector2.Right.Rotated(Rotation);
        var movement = forward * speed * (float)delta;
        GlobalPosition += movement;

        // Push all enemies currently overlapping toward the target line
        foreach (var area in GetOverlappingAreas())
        {
            if (area is Enemy enemy)
            {
                enemiesPushed.Add(enemy);
                // Push enemy toward the center line (target position)
                var toTarget = (targetPosition - enemy.GlobalPosition);
                if (toTarget.Length() > 10f)
                {
                    var pushDir = toTarget.Normalized();
                    enemy.GlobalPosition += pushDir * pushStrength * (float)delta;
                }
            }
        }

        // Check if we've reached or passed the target
        var distanceToTarget = GlobalPosition.DistanceTo(targetPosition);
        if (distanceToTarget < 30f)
        {
            reachedTarget = true;
            GlobalPosition = targetPosition;

            // Deal damage to all enemies at the crush point
            DealCrushDamage();
            FadeAndFree();
        }
    }

    private void DealCrushDamage()
    {
        // Damage all enemies that were pushed by this wall
        foreach (var enemy in enemiesPushed)
        {
            if (IsInstanceValid(enemy) && enemiesHit.Add(enemy))
            {
                enemy.TakeDamage(damage);
            }
        }

        // Also damage any enemies still overlapping
        foreach (var area in GetOverlappingAreas())
        {
            if (area is Enemy enemy && enemiesHit.Add(enemy))
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        // Track enemies but don't damage yet - damage happens at crush point
        if (area is Enemy enemy)
        {
            enemiesPushed.Add(enemy);
        }
    }

    private async void FadeAndFree()
    {
        var tween = GetTree().CreateTween();
        tween.TweenProperty(this, "modulate", new Color(1, 1, 1, 0), 0.3f);
        await ToSignal(tween, Tween.SignalName.Finished);
        CallDeferred("queue_free");
    }

    public void Init(float damage, Vector2 targetPosition, float width)
    {
        this.damage = damage;
        this.targetPosition = targetPosition;
    }
}
