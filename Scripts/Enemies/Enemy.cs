using Godot;

public abstract partial class Enemy : Area2D
{
    [Export] private float moveSpeed = 50;
    [Export] private float attackCooldown = 2;
    [Export] private float attackRange = 100;
    [Export] private float health = 2;
    protected Player player;
    private Timer attackTimer;

    public override void _Ready()
    {
        attackTimer = new Timer();
        AddChild(attackTimer);
        attackTimer.WaitTime = attackCooldown;
        attackTimer.OneShot = true;
        attackTimer.Start();
        player = GetNode<Player>("../../Player");
        
        AddToGroup("Enemies");
    }

    public override void _Process(double delta)
    {
        if (attackRange >= GlobalPosition.DistanceTo(player.GlobalPosition))
        {
            if (attackTimer.TimeLeft <= 0)
            {
                Attack();
                attackTimer.Start();
            }
        }
        else
        {
            player = GetNode<Player>("../../Player");
            var direction = (player.GlobalPosition - GlobalPosition).Normalized();
            Position += direction * moveSpeed * (float)delta;
        }
    }

    public void TakeDamage(float damage)
    {
        GD.Print("TakeDamage");
        health -= damage;
        if (health <= 0)
        {
            RemoveFromGroup("Enemies");
            QueueFree();
        }
    }
    
    public abstract void Attack();
}