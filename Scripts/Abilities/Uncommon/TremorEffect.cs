using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Uncommon;

public partial class TremorEffect : Area2D
{
    private float damage;
    private int bleedStacks;
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
            CallDeferred("queue_free");
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is Enemy enemy)
        {
            enemy.TakeDamage(damage);
            enemy.AddActiveDot(DamageOverTime.GetBleed(bleedStacks));
            enemy.AddSlow(slow, slowDuration);
        }
    }

    public void Init((float damage, int bleedStacks, float slow, float slowDuration) stats)
    {
        damage = stats.damage;
        bleedStacks = stats.bleedStacks;
        slow = stats.slow;
        slowDuration = stats.slowDuration;
    }
}