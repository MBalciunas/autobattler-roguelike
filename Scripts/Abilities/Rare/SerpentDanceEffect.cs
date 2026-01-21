using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Rare;

public partial class SerpentDanceEffect : Node2D
{
    public override async void _Ready()
    {
        var tween = GetTree().CreateTween();
        tween.TweenProperty(this, "modulate", new Color(0, 0, 0, 0), 0.2);

        await ToSignal(tween, Tween.SignalName.Finished);
        CallDeferred("queue_free");
    }
}