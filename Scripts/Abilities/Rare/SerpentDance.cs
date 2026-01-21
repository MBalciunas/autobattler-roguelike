using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Rare;

public partial class SerpentDance : Ability
{
    [Export] private PackedScene serpentDanceEffectScene;
    private float range = 260f;
    private int strikeTimes = 10;
    private Tween tween;

    public override void _Ready() { }

    protected override async void ExecuteAbility()
    {
        var enemiesInRange = GlobalManager.GetEnemiesSortedByClosest()
            .Where(e => e.GlobalPosition.DistanceTo(GlobalPosition) <= range).ToList();

        if (enemiesInRange.Count == 0) return;

        var stats = GetStats();
        //TODO need to check if enemy dies/removed, to skip. Maybe new ones entered range?
        for (int i = 0; i < strikeTimes; i++)
        {
            var target = enemiesInRange[i % enemiesInRange.Count];
            var effect = serpentDanceEffectScene.Instantiate<SerpentDanceEffect>();
            var direction = (target.GlobalPosition - GlobalPosition).Normalized();
            effect.GlobalPosition = GlobalPosition + direction * 20;
            effect.Rotation = direction.Angle();
            GetTree().Root.GetNode("MainLevel").AddChild(effect);
            target.TakeDamage(stats.damage);
            target.AddActiveDot(DamageOverTime.GetPoison(stats.poisonStacks));

            if (enemiesInRange.Count > i)
            {
                GlobalManager.playerState.AddShield(stats.shieldPerEnemy);
            }
            await ToSignal(GetTree().CreateTimer(0.05f), SceneTreeTimer.SignalName.Timeout);
        }
    }
}