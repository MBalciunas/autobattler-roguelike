using AutoBattlerRoguelike.Scripts.Abilities;
using Godot;

public partial class ToxicDartProjectile : Area2D
{
    private float damage;
    private int poisonStacks;
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
            enemy.AddActiveDot(DamageOverTime.GetPoison(poisonStacks));
            CallDeferred("queue_free");
        }
    }

    public void Init((float damage, int poisonStacks) stats)
    {
        damage = stats.damage;
        poisonStacks = stats.poisonStacks;
    }
}