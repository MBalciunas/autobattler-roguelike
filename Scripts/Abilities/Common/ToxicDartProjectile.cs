using AutoBattlerRoguelike.Scripts.Abilities;
using Godot;

public partial class ToxicDartProjectile : Area2D
{
    private float damage;
    private float poisonDamage;
    private float poisonDuration;
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
            enemy.AddActiveDot(new DamageOverTime(poisonDamage, poisonDuration, DamageType.Poison));
            QueueFree();
        }
    }

    public void Init((float cleaveDamage, float bleedDamage, int bleedDuration) stats)
    {
        damage = stats.cleaveDamage;
        poisonDamage = stats.bleedDamage;
        poisonDuration = stats.bleedDuration;
    }
}