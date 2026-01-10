using System;
using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Uncommon;

public partial class IronWing : Ability
{
    [Export] private PackedScene ironWingEffectScene;
    private float range = 230f;
    private Tween tween;

    public override void _Ready() { }

    protected override void ExecuteAbility()
    {
        var effect = ironWingEffectScene.Instantiate<IronWingEffect>();
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
        // Match Data/Abilities.json: damage 4/10/20; shield 2/5/10 per enemy hit
        return level switch
        {
            1 => (damage: 4f, shield: 1f),
            2 => (damage: 10f, shield: 2f),
            3 => (damage: 20f, shield: 5f),
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }
}