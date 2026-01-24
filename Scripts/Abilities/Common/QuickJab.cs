using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class QuickJab : Ability
{
    [Export] private PackedScene effectScene;
    private QuickJabEffect effect;
    private float jabDistance = 150f;

    protected override void ExecuteAbility()
    {
        var enemy = GlobalManager.GetEnemiesSortedByClosest().FirstOrDefault();

        var stats = GetStats();
        effect = effectScene.Instantiate<QuickJabEffect>();
        effect.Init((stats.damage, stats.width));

        Vector2 direction = Vector2.Right;
        if (enemy != null)
        {
            direction = (enemy.GlobalPosition - GlobalPosition).Normalized();
        }

        // Width scale factor (base width is 40, so scale relative to that)
        float widthScale = stats.width / 40f;

        // Start at player position, will extend outward
        effect.GlobalPosition = GlobalPosition;
        effect.Rotation = direction.Angle();
        effect.Scale = new Vector2(0.1f, widthScale); // Start small in extending direction, apply width

        GetTree().Root.GetNode("MainLevel").AddChild(effect);

        var tween = GetTree().CreateTween();
        tween.SetParallel(true);

        // Extend the effect outward
        tween.TweenProperty(effect, "scale", new Vector2(1f, widthScale), 0.08f)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Quad);

        // Move position so it extends from player toward enemy
        var endPosition = GlobalPosition + direction * jabDistance;
        tween.TweenProperty(effect, "global_position", endPosition, 0.08f)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Quad);

        tween.SetParallel(false);
        tween.TweenInterval(0.07f); // Brief hold at full extension
        tween.TweenCallback(Callable.From(() =>
        {
            if (effect != null && IsInstanceValid(effect))
            {
                effect.QueueFree();
            }
        }));
    }
}
