using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class HeavySlam : Ability
{
    [Export] private PackedScene effectScene;

    protected override void ExecuteAbility()
    {
        var enemy = GlobalManager.GetEnemiesSortedByClosest().FirstOrDefault();
        if (enemy == null) return;

        var stats = GetStats();
        var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();

        var effect = effectScene.Instantiate<HeavySlamEffect>();
        effect.Init((stats.damage, stats.knockbackStrength, stats.range, direction));
        // Position effect so it extends forward from player (offset by half the range)
        effect.GlobalPosition = GlobalPosition + direction * (stats.range / 2);
        effect.Rotation = direction.Angle();

        GetTree().Root.GetNode("MainLevel").AddChild(effect);
    }
}
