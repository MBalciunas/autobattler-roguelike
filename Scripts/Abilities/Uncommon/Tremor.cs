using System;
using System.Linq;
using AutoBattlerRoguelike.Scripts.Abilities.Common;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Uncommon;

public partial class Tremor : Ability
{
    [Export] private PackedScene tremorEffectScene;
    private Tween tween;

    public override void _Ready() { }

    protected override void ExecuteAbility()
    {
        var enemy = GlobalManager.GetEnemiesSortedByClosest().FirstOrDefault();

        var effect = tremorEffectScene.Instantiate<TremorEffect>();
        effect.Init(GetStatsForLevel(Level));
        effect.GlobalPosition = GlobalPosition + Vector2.Left * 20;
        if (enemy != null)
        {
            var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();
            effect.GlobalPosition = GlobalPosition + direction * 20;
            effect.Rotation = direction.Angle();
        }

        GetTree().Root.GetNode("MainLevel").AddChild(effect);
    }


    private (float damage, float bleedDamage, float bleedDuration, float slow, float slowDuration) GetStatsForLevel(int level)
    {
        return level switch
        {
            1 => (damage: 20f, bleedDamage: 4f, bleedDuration: 4, slow: 0.3f,  slowDuration: 3),
            2 => (damage: 50f, bleedDamage: 8f, bleedDuration: 4, slow: 0.4f,  slowDuration: 4),
            3 => (damage: 120f, bleedDamage: 15f, bleedDuration: 4, slow: 0.6f,  slowDuration: 6),
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }
}