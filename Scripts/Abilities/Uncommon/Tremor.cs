using System.Linq;
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

        var stats = GetStats();
        var effect = tremorEffectScene.Instantiate<TremorEffect>();
        effect.Init((stats.damage, stats.bleedStacks, stats.slow, stats.slowDuration));
        effect.GlobalPosition = GlobalPosition + Vector2.Left * 20;
        if (enemy != null)
        {
            var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();
            effect.GlobalPosition = GlobalPosition + direction * 20;
            effect.Rotation = direction.Angle();
        }

        GetTree().Root.GetNode("MainLevel").AddChild(effect);
    }
}