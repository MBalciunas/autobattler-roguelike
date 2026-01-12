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

        cleaveEffect = cleaveEffectScene.Instantiate<CleaveEffect>();
        cleaveEffect.Init(GetStatsForLevel(Level));
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
        cleaveEffect.QueueFree();
        cleaveEffect = null;
        tween.Dispose();
    }

    private (float damage, int bleedStacks) GetStatsForLevel(int level)
    {
        return level switch
        {
            1 => (damage: 1f, bleedStacks: 1),
            2 => (damage: 3f, bleedStacks: 2),
            3 => (damage: 10f, bleedStacks: 3),
        };
    }
}