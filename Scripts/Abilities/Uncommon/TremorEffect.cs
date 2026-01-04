using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class TremorEffect : Area2D
{
    private float damage;
    private float bleedDamage;
    private float bleedDuration;
    private float slow;
    private float slowDuration;
    private float range = 400f;
    private float speed = 300f;
    private float distanceTraveled = 0;

    public override void _Process(double delta)
    {
        var travelDistance = Vector2.Right.Rotated(Rotation) * speed * (float)delta;
        distanceTraveled += travelDistance.Length();
        GlobalPosition += travelDistance;

        if (distanceTraveled > range)
        {
            QueueFree();
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is Enemy enemy)
        {
            enemy.TakeDamage(damage);
            enemy.AddActiveDot(new DamageOverTime(bleedDamage, bleedDuration, ElementType.Bleed));
            enemy.AddSlow(slow, slowDuration);
        }
    }

    public void Init((float damage, float bleedDamage, float bleedDuration, float slow, float slowDuration) stats)
    {
        damage = stats.damage;
        bleedDamage = stats.bleedDamage;
        bleedDuration = stats.bleedDuration;
        slow = stats.slow;
        slowDuration = stats.slowDuration;
    }
}