using Godot;

namespace AutoBattlerRoguelike.Scripts.UI;

public partial class FloatingText : Label
{
    private static PackedScene _scene;

    public static void Spawn(Node parent, Vector2 position, string text, Color color)
    {
        _scene ??= GD.Load<PackedScene>("res://Scenes/UI/FloatingText.tscn");

        var instance = _scene.Instantiate<FloatingText>();
        instance.Text = text;
        instance.Modulate = color;
        instance.GlobalPosition = position;
        parent.AddChild(instance);
    }

    public static void SpawnGold(Node parent, Vector2 position, int amount)
    {
        var text = amount > 0 ? $"+{amount} gold" : $"{amount} gold";
        Spawn(parent, position, text, new Color(1f, 0.85f, 0.2f)); // Yellow/gold color
    }

    public override void _Ready()
    {
        var tween = CreateTween();
        tween.SetParallel(true);

        // Float upward
        tween.TweenProperty(this, "position", Position + new Vector2(0, -40), 0.6f)
            .SetEase(Tween.EaseType.Out)
            .SetTrans(Tween.TransitionType.Quad);

        // Fade out
        tween.TweenProperty(this, "modulate:a", 0f, 0.6f)
            .SetEase(Tween.EaseType.In)
            .SetTrans(Tween.TransitionType.Quad);

        tween.SetParallel(false);
        tween.TweenCallback(Callable.From(QueueFree));
    }
}
