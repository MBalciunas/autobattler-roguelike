using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Uncommon;

public partial class FlurryProjectile : Area2D
{
    private float damage;
    private float speed = 900;

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
            CallDeferred("queue_free");
        }
    }

    public void Init(float damage)
    {
        this.damage = damage;
    }
}
