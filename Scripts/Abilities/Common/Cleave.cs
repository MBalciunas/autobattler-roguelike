using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class Cleave : Ability
{
    [Export] private PackedScene cleaveEffectScene;
    private CleaveEffect cleaveEffect;
    private Tween tween;

    public override void _Ready() { }

    protected override void ExecuteAbility()
    {
        var enemy = GlobalManager.GetEnemiesSortedByClosest().FirstOrDefault();

        var stats = GetStats();
        cleaveEffect = cleaveEffectScene.Instantiate<CleaveEffect>();
        cleaveEffect.Init((stats.damage, stats.bleedStacks));
        cleaveEffect.GlobalPosition = GlobalPosition + Vector2.Left * 20;
        if (enemy != null)
        {
            var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();
            cleaveEffect.GlobalPosition = GlobalPosition + direction * 20;
            cleaveEffect.Rotation = direction.Angle() - Mathf.DegToRad(60);
        }

        GetTree().Root.GetNode("MainLevel").AddChild(cleaveEffect);
        tween = GetTree().CreateTween();
        tween.TweenProperty(cleaveEffect, "rotation", cleaveEffect.Rotation + Mathf.DegToRad(120), 0.2);
        tween.Finished += TweenOnFinished;
    }

    private void TweenOnFinished()
    {
        if (cleaveEffect == null) return;
        cleaveEffect.CallDeferred("queue_free");
        cleaveEffect = null;
        tween.Dispose();
    }
}