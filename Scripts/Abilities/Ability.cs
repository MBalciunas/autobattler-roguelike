using AutoBattlerRoguelike.Scripts.Abilities;
using Godot;

public abstract partial class Ability : Node2D
{
    public int Level = 1;
    public AbilityResource Resource { get; set; }

    // Damage bonus captured at ability start (from StalkingPounce)
    protected float DamageBonus { get; private set; }

    public async void Execute()
    {
        // Capture and consume the bonus at start so all hits from this ability get it
        DamageBonus = GlobalManager.playerState.NextAbilityDamageBonus;
        GlobalManager.playerState.NextAbilityDamageBonus = 0;

        ExecuteAbility();
        await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);
        EmitSignal(SignalName.Finished, this);
    }

    protected abstract void ExecuteAbility();
    [Signal] public delegate void FinishedEventHandler(Ability ability);

    protected AbilityLevelStats GetStats()
    {
        var baseStats = Resource.GetStats(Level);
        float multiplier = 1 + DamageBonus;
        return new AbilityLevelStats
        {
            damage = baseStats.damage * multiplier,
            poisonedDamage = baseStats.poisonedDamage * multiplier,
            burningDamage = baseStats.burningDamage * multiplier,
            bleedStacks = baseStats.bleedStacks,
            poisonStacks = baseStats.poisonStacks,
            burnStacks = baseStats.burnStacks,
            slow = baseStats.slow,
            slowDuration = baseStats.slowDuration,
            shield = baseStats.shield,
            shieldPerEnemy = baseStats.shieldPerEnemy,
            duration = baseStats.duration,
            knockbackStrength = baseStats.knockbackStrength,
            knockbackRadius = baseStats.knockbackRadius,
            explosionRadius = baseStats.explosionRadius,
            nextAbilityDamageBonus = baseStats.nextAbilityDamageBonus,
            projectileCount = baseStats.projectileCount,
            goldOnKillChance = baseStats.goldOnKillChance,
            ricochets = baseStats.ricochets,
            width = baseStats.width,
            range = baseStats.range
        };
    }
}
