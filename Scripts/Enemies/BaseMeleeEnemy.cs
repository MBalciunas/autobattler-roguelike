using Godot;

public partial class BaseMeleeEnemy : Enemy
{
    [Export] private float damage;
    Tween tween;


    public override void _Ready()
    {
        damage *= Mathf.Pow(1.15f, GlobalManager.Level - 1);
        base._Ready();
    }

    public override void Attack()
    {
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is Player player)
        {
            player.TakeDamage(damage);
            var direction = (GlobalPosition - player.GlobalPosition).Normalized();
            Knockback(100f, direction);
        }
    }
}
