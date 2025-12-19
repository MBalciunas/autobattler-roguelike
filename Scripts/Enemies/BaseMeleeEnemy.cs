using Godot;

public partial class BaseMeleeEnemy : Enemy
{
    [Export] private float damage;
    Tween tween;

    public override void Attack()
    {
        var tweener = GetTree().CreateTween();
        var originalPos = Position;
        tweener.TweenProperty(this, "position", player.GlobalPosition, 0.05f);
        tweener.Parallel().TweenProperty(this, "scale", new Vector2(0.25f, 0.25f), 0.05f);
        
        player.TakeDamage(damage);
        
        tweener.TweenProperty(this, "position", originalPos, 0.1f);
        tweener.Parallel().TweenProperty(this, "scale", new Vector2(1f, 1f), 0.1f);
        tweener.Dispose();
    }
}
