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

        if (enemy != null)
        {
            cleaveEffect = cleaveEffectScene.Instantiate<CleaveEffect>();
            cleaveEffect.Init(GetStatsForLevel(Level));
            var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();
            cleaveEffect.GlobalPosition = GlobalPosition + direction * 20;
            cleaveEffect.Rotation = direction.Angle() - Mathf.DegToRad(60);
            GetTree().Root.GetNode("MainLevel").AddChild(cleaveEffect);
            tween = GetTree().CreateTween();
            tween.TweenProperty(cleaveEffect, "rotation", cleaveEffect.Rotation + Mathf.DegToRad(120), 0.2);
            tween.Finished += TweenOnFinished;
        }
    }

    private void TweenOnFinished()
    {
        if (cleaveEffect == null) return;
        cleaveEffect.QueueFree();
        cleaveEffect = null;
        tween.Dispose();
    }

    private (float cleaveDamage, float bleedDamage, int bleedDuration) GetStatsForLevel(int level)
    {
        return level switch
        {
            1 => (cleaveDamage: 1f, bleedDamage: 0.2f, bleedDuration: 4),
            2 => (cleaveDamage: 2f, bleedDamage: 0.5f, bleedDuration: 5),
            3 => (cleaveDamage: 3f, bleedDamage: 1f, bleedDuration: 6),
            _ => (cleaveDamage: 1.0f, bleedDamage: 0.2f, bleedDuration: 4)
        };
    }
}