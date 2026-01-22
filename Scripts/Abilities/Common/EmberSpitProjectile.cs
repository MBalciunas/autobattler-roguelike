using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class EmberSpitProjectile : Area2D
{
    private float damage;
    private int burnStacks;
    private float explosionRadius;
    private float speed = 700;

    private Sprite2D explosionSprite;

    public override void _Ready()
    {
        explosionSprite = GetNode<Sprite2D>("ExplosionSprite");
    }

    public override void _Process(double delta)
    {
        Vector2 forward = Vector2.Right.Rotated(Rotation);
        GlobalPosition += forward * speed * (float)delta;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is Enemy)
        {
            Explode();
        }
    }

    private void Explode()
    {
        var enemiesInRadius = GlobalManager.GetEnemiesSortedByClosest()
            .Where(e => e.GlobalPosition.DistanceTo(GlobalPosition) <= explosionRadius)
            .ToList();

        foreach (var enemy in enemiesInRadius)
        {
            enemy.TakeDamage(damage);
            enemy.AddActiveDot(DamageOverTime.GetBurn(burnStacks));
        }

        if (explosionSprite != null)
        {
            explosionSprite.Scale = Vector2.One * explosionRadius / 64f;
            explosionSprite.Modulate = new Color(1, 0.5f, 0, 0.7f);
            var tween = GetTree().CreateTween();
            tween.TweenProperty(explosionSprite, "modulate", new Color(1, 0.5f, 0, 0), 0.3f);
            tween.TweenCallback(Callable.From(QueueFree));
        }
        else
        {
            QueueFree();
        }

        SetProcess(false);
    }

    public void Init((float damage, int burnStacks, float explosionRadius) stats)
    {
        damage = stats.damage;
        burnStacks = stats.burnStacks;
        explosionRadius = stats.explosionRadius;
    }
}
