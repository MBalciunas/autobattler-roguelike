using System;
using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Uncommon;

public partial class DragonBreath : Ability
{
    [Export] private PackedScene dragonBreathEffectScene;
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

    private (float damage, float poisonedDamage)
        GetStatsForLevel(int level)
    {
        // Match Data/Abilities.json: base damage 5/10/25; if poisoned 10/20/50
        return level switch
        {
            1 => (damage: 5f, poisonedDamage: 10f),
            2 => (damage: 10f, poisonedDamage: 20f),
            3 => (damage: 25f, poisonedDamage: 50f),
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }
}