using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Uncommon;

public partial class VenomFang : Ability
{
    [Export] private PackedScene venomFangEffectScene;
    private float range = 230f;
    private Tween tween;

    public override void _Ready() { }

    protected override void ExecuteAbility()
    {
        var closest3Enemies = GlobalManager.GetEnemiesSortedByClosest().Take(3).ToList();

        var stats = GetStats();
        foreach (var enemy in closest3Enemies)
        {
            var effect = venomFangEffectScene.Instantiate<VenomFangEffect>();
            var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();
            effect.GlobalPosition = GlobalPosition + direction * 20;
            effect.Rotation = direction.Angle();
            if (enemy.GlobalPosition.DistanceTo(GlobalPosition) <= range)
            {
                GetTree().Root.GetNode("MainLevel").AddChild(effect);
                enemy.TakeDamage(stats.damage);
                enemy.AddActiveDot(DamageOverTime.GetPoison(stats.poisonStacks));
                enemy.AddActiveDot(DamageOverTime.GetBleed(stats.bleedStacks));
            }
        }
    }
}