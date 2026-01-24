using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Uncommon;

public partial class InfernoKick : Ability
{
    [Export] private PackedScene infernoKickEffectScene;

    public override void _Ready() { }

    protected override void ExecuteAbility()
    {
        var stats = GetStats();
        var effect = infernoKickEffectScene.Instantiate<InfernoKickEffect>();
        effect.Init((stats.damage, stats.burnStacks));
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
}
