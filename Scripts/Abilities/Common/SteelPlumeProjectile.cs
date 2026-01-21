using Godot;

public partial class SteelPlumeProjectile : Area2D
{
    private float damage;
    private float slow;
    private float slowDuration;
    private float speed = 800;

    public override void _Process(double delta)
    {
        Vector2 forward = Vector2.Right.Rotated(Rotation);
        GlobalPosition += forward * speed * (float)delta;
    }
    
    private void OnAreaEntered(Area2D area)
    {
        if (area is Enemy enemy)
        {
            enemy.TakeDamage(damage);
            enemy.AddSlow(slow, slowDuration);
        }
    }

    public void Init((float damage, float slow, float slowDuration) stats)
    {
        damage = stats.damage;
        slow = stats.slow;
        slowDuration = stats.slowDuration;
    }
}