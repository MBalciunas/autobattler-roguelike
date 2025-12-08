using System.Linq;
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
        health -= damage;
        if (health <= 0)
        {
            RemoveFromGroup("Enemies");
            GD.Print(GetTree().GetNodesInGroup("Enemies").Count);
            GD.Print(GlobalManager.IsEnemiesSpawning);
            if (GetTree().GetNodesInGroup("Enemies").Count == 0 && !GlobalManager.IsEnemiesSpawning)
            {
                GameManager.Instance.LoadNextLevel();
            }

            CallDeferred("queue_free");
        }
    }
    
    public abstract void Attack();
}