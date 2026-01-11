using System;
using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Rare;

public partial class DragonTail : Ability
{
    [Export] private PackedScene dragonTailEffectScene;
    private Tween tween;

    public override void _Ready() { }

    protected override void ExecuteAbility()
    {
        var effect = dragonTailEffectScene.Instantiate<DragonTailEffect>();
        (float damage, float shield) stats = GetStatsForLevel(Level);
        effect.Init(stats.damage);
        effect.GlobalPosition = GlobalPosition + Vector2.Left * 20;

        var enemy = GlobalManager.GetEnemiesSortedByClosest().FirstOrDefault();

        if (enemy != null)
        {
            var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();
            effect.GlobalPosition = GlobalPosition + direction * 20;
            effect.Rotation = direction.Angle();
        }
        
        GlobalManager.playerState.AddShield(stats.shield);
        GetTree().Root.GetNode("MainLevel").AddChild(effect);

    }

    private (float damage, float shield)
        GetStatsForLevel(int level)
    {
        return level switch
        {
            1 => (damage: 5f, shield: 5f),
            2 => (damage: 12f, shield: 10f),
            3 => (damage: 30f, shield: 20f),
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }
}