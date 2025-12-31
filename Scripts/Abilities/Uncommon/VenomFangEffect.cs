using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Uncommon;

public partial class VenomFangEffect : Node2D
{
    public override void _Ready()
    {
        var tween = GetTree().CreateTween();
        tween.TweenProperty(this, "modulate", new Color(0, 0, 0, 0), 0.5);
        tween.Finished += TweenOnFinished;
    }

    private void TweenOnFinished()
    {
        QueueFree();
    }
}