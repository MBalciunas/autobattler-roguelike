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
        var stats = GetStats();
        var effect = dragonTailEffectScene.Instantiate<DragonTailEffect>();
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
}