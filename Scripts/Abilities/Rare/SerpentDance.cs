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

    
    private (float damage, int poisonStacks, float shieldPerEnemy) GetStatsForLevel(int level)
    {
        return level switch
        {
            1 => (damage: 5f, poisonStacks: 2, shieldPerEnemy: 1f),
            2 => (damage: 10f, poisonStacks: 3, shieldPerEnemy: 2f),
            3 => (damage: 20f, poisonStacks: 5, shieldPerEnemy: 5f),
        };
    }
}