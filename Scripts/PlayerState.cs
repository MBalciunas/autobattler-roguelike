using System.Linq;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace AutoBattlerRoguelike.Scripts;

[GlobalClass]
public partial class PlayerState : Resource
{
    public PlayerStatFloat Health { get; private set; }
    public PlayerStatFloat Shield { get; private set; }
    public PlayerStatFloat MaxHealth { get; private set; }
    public PlayerStatFloat Damage { get; private set; }
    public PlayerStatFloat CritChance { get; private set; }
    public PlayerStatFloat CritDamage { get; private set; }
    public PlayerStatFloat Armor { get; private set; }
    public PlayerStatFloat Lifesteal { get; private set; }
    public PlayerStatInt Gold { get; private set; }

    // Progression
    public PlayerStatInt Level { get; private set; }
    public PlayerStatInt CurrentXP { get; private set; }

    // XP requirements table keyed by target level (e.g., 2 => XP needed from L1->L2)
    private static readonly System.Collections.Generic.Dictionary<int, int> XpTable = new System.Collections.Generic.Dictionary<int, int>
    {
        { 2, 4 },
        { 3, 10 },
        { 4, 16 },
        { 5, 22 },
        { 6, 30 },
        { 7, 45 },
        { 8, 60 },
        { 9, 80 },
        { 10, 100 },
    };

    public Array<PlayerAbilityResource> AbilitiesInLoop { get; private set; }

    [Signal]
    public delegate void OnAbilitiesChangedEventHandler(Array<PlayerAbilityResource> AbilitiesInLoop);

    [Signal]
    public delegate void OnLevelChangedEventHandler(int level);

    [Signal]
    public delegate void OnXPChangedEventHandler(int currentXP, int xpToNextLevel);

    public PlayerState()
    {
        InitializeStats();
    }

    public void InitializeStats()
    {
        MaxHealth = new PlayerStatFloat(200);
        Health = new PlayerStatFloat(MaxHealth.Value).OnMin(_ => GameManager.Instance.RestartGame());
        Shield = new PlayerStatFloat(0);
        Health.SetMax(MaxHealth.Value);
        Gold = new PlayerStatInt(0);
        Damage = new PlayerStatFloat(0);
        CritChance = new PlayerStatFloat(10);
        CritDamage = new PlayerStatFloat(1.5f);
        Armor = new PlayerStatFloat(0);
        Lifesteal = new PlayerStatFloat(0);

        Level = new PlayerStatInt(1);
        CurrentXP = new PlayerStatInt(0);

        // React to changes for UI updates
        Level.OnValueChanged += OnLevelValueChanged;
        CurrentXP.OnValueChanged += OnXPValueChanged;

        AbilitiesInLoop =
        [
            new PlayerAbilityResource(GlobalManager.Abilities[AbilityName.ToxicPool]),
        ];

        // Emit initial values for UI that might subscribe late
        EmitSignal(SignalName.OnLevelChanged, Level.Value);
        EmitSignal(SignalName.OnXPChanged, CurrentXP.Value, GetXpToNextLevel());
    }

    private void OnLevelValueChanged(int value)
    {
        EmitSignal(SignalName.OnLevelChanged, value);
        // When level changes, XP to next also changes
        EmitSignal(SignalName.OnXPChanged, CurrentXP.Value, GetXpToNextLevel());
    }

    private void OnXPValueChanged(int value)
    {
        EmitSignal(SignalName.OnXPChanged, value, GetXpToNextLevel());
    }

    public void ResetHealth()
    {
        Health.Set(MaxHealth.Value);
    }

    public void Heal(float amount) => Health.Add(amount);

    public void TakeDamage(float amount)
    {
        var amountLeft = amount;
        if (Shield.Value > 0)
        {
            amountLeft -= Shield.Value;
            Shield.Add(-amount);
        }
        if (amountLeft > 0)
        {
            Health.Add(-amountLeft);
        }
    }

    public void AddGold(int amount)
    {
        Gold.Add(amount);
    }

    public int GetTraitCount(AbilityTrait trait)
    {
        return AbilitiesInLoop.Count(a => a.AbilityResource.Traits.Contains(trait));
    }

    public void IncreaseDamage(float amount) => Damage.Add(amount);

    public void AddAbility(AbilityResource abilityResource)
    {
        // Backwards-compatible method: perform addition without transactional feedback
        TryAddAbility(abilityResource);
    }

    public bool TryAddAbility(AbilityResource abilityResource)
    {
        var currentAbility = AbilitiesInLoop.ToList()
            .FirstOrDefault(a => a.AbilityResource.AbilityName == abilityResource.AbilityName);

        if (currentAbility == null)
        {
            // Enforce capacity: number of distinct abilities cannot exceed Level
            if (AbilitiesInLoop.Count >= Level.Value)
            {
                // Not enough capacity to add a new ability
                GD.Print("Ability loop is full for current level. Level up to add more abilities.");
                return false;
            }

            AbilitiesInLoop.Add(new PlayerAbilityResource(abilityResource));
        }
        else
        {
            // Stacking copies of an existing ability is allowed regardless of capacity
            currentAbility.AddCopy();
        }

        EmitSignal(SignalName.OnAbilitiesChanged, AbilitiesInLoop);
        return true;
    }

    // ================== Progression Logic ==================
    public void AddXP(int amount)
    {
        if (amount <= 0) return;
        if (Level.Value >= 10) return; // no XP past max level
        CurrentXP.Add(amount);

        // Handle multiple level-ups if a lot of XP is added
        while (Level.Value < 10 && CurrentXP.Value >= GetXpRequired(Level.Value))
        {
            CurrentXP.Subtract(GetXpRequired(Level.Value));
            Level.Add(1);
        }

        if (Level.Value >= 10)
        {
            // Clear XP at cap for cleaner UI/state
            CurrentXP.Value = 0;
        }
    }

    public int GetXpToNextLevel()
    {
        if (Level.Value >= 10) return 0;
        return GetXpRequired(Level.Value) - CurrentXP.Value;
    }

    public int GetXpRequired(int level)
    {
        // Use custom XP table; level is current level, returns XP to reach next level
        if (level >= 10) return 0; // at cap, no further XP required
        return XpTable.TryGetValue(level + 1, out var req) ? req : 0;
    }

    public int SpendGoldForXP(int goldToSpend, float rateGoldPerXP = 1f)
    {
        if (goldToSpend <= 0) return 0;
        var affordable = Mathf.Min(goldToSpend, Gold.Value);
        if (affordable <= 0) return 0;

        var xp = Mathf.FloorToInt(affordable / rateGoldPerXP);
        if (xp <= 0) return 0;

        Gold.Subtract(affordable);
        AddXP(xp);
        return xp;
    }
}