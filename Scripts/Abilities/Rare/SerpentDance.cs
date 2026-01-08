using System;
using System.Linq;
using AutoBattlerRoguelike.Scripts.Abilities.Uncommon;
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
        
        var stats = GetStatsForLevel(Level);
        for (int i = 0; i < strikeTimes; i++)
        {
            var target = enemiesInRange[i % enemiesInRange.Count];
            var effect = serpentDanceEffectScene.Instantiate<SerpentDanceEffect>();
            var direction = (target.GlobalPosition - GlobalPosition).Normalized();
            effect.GlobalPosition = GlobalPosition + direction * 20;
            effect.Rotation = direction.Angle();
            GetTree().Root.GetNode("MainLevel").AddChild(effect);
            target.TakeDamage(stats.damage);
            target.AddActiveDot(new DamageOverTime(stats.poisonDamage, stats.poisonDuration, ElementType.Poison));

            if (enemiesInRange.Count > i)
            {
                GlobalManager.playerState.Shield.Add(stats.shieldPerEnemy);
            }
            await ToSignal(GetTree().CreateTimer(0.05f), SceneTreeTimer.SignalName.Timeout);
        }
    }

    private (float damage, float poisonDamage, float poisonDuration, float shieldPerEnemy)
        GetStatsForLevel(int level)
    {
        return level switch
        {
            1 => (damage: 10f, poisonDamage: 4f, poisonDuration: 10f, shieldPerEnemy: 10f),
            2 => (damage: 25f, poisonDamage: 7f, poisonDuration: 10f, shieldPerEnemy: 15f),
            3 => (damage: 45f, poisonDamage: 12f, poisonDuration: 10f, shieldPerEnemy: 30f),
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }
}