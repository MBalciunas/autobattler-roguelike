using System;
using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Uncommon;

public partial class DragonBreath : Ability
{
    [Export] private PackedScene dragonBreathEffectScene;
    private float range = 230f;
    private Tween tween;

    public override void _Ready() { }

    protected override void ExecuteAbility()
    {
        var effect = dragonBreathEffectScene.Instantiate<DragonBreathEffect>();
        effect.Init(GetStatsForLevel(Level));
        effect.GlobalPosition = GlobalPosition + Vector2.Left * 20;

        var enemy = GlobalManager.GetEnemiesSortedByClosest().FirstOrDefault();

        if (enemy != null)
        {
            var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();
            effect.GlobalPosition = GlobalPosition + direction * 20;
            effect.Rotation = direction.Angle();
        }
        GetTree().Root.GetNode("MainLevel").AddChild(effect);

    }

    private (float damage, float shield)
        GetStatsForLevel(int level)
    {
        return level switch
        {
            1 => (damage: 30f, poisonedDamaged: 40f),
            2 => (damage: 60f, poisonedDamaged: 90f),
            3 => (damage: 150f, poisonedDamaged: 250f),
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }
}