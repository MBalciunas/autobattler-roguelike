using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Rare;

public partial class ToxicPool : Ability
{
    [Export] private PackedScene effectScene;

    protected override void ExecuteAbility()
    {
        var enemy = GlobalManager.GetEnemiesSortedByClosest().FirstOrDefault();

        if (enemy != null)
        {
            var stats = GetStats();
            var toxicPool = effectScene.Instantiate<ToxicPoolEffect>();
            toxicPool.Init((stats.damage, stats.poisonStacks, stats.slow, stats.duration));
            toxicPool.GlobalPosition = enemy.Position;
            GetTree().Root.GetNode("MainLevel").AddChild(toxicPool);
        }
    }
}