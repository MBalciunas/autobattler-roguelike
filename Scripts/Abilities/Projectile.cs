using Godot;

public partial class Projectile : Area2D
{
    public float damage;
    [Export] private float speed;

    public override void _Process(double delta)
    {
        Vector2 forward = Vector2.Right.Rotated(Rotation);
        GlobalPosition += forward * speed * (float)delta;
    }
    
    private void OnAreaEntered(Area2D area)
    {
        GD.Print("OnAreaEntered");
        if (area is Enemy enemy)
        {
            enemy.TakeDamage(damage);
            QueueFree();
        }
    }
}