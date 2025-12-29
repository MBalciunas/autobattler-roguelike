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
            enemy.AddActiveDot(new DamageOverTime(poisonDamage, poisonDuration, ElementType.Poison));
            QueueFree();
        }
    }

    public void Init((float damage, float poisonDamage, int poisonDuration) stats)
    {
        damage = stats.damage;
        poisonDamage = stats.poisonDamage;
        poisonDuration = stats.poisonDuration;
    }
}