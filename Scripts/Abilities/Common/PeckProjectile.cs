using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class PeckProjectile : Area2D
{
    private float damage;
    private float shieldOnHit;
    private float speed = 700f;

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
            GlobalManager.playerState.AddShield(shieldOnHit);
            QueueFree();
        }
    }

    public void Init((float damage, float shield) stats)
    {
        damage = stats.damage;
        shieldOnHit = stats.shield;
    }
}
