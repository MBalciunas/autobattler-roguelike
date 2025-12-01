using Godot;

public abstract partial class Enemy : Node2D
{
    [Export] private float moveSpeed = 50;
    [Export] private float attackCooldown = 2;
    [Export] private float attackRange = 100;
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

    public abstract void Attack();
}