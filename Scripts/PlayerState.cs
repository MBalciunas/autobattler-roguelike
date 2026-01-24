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
    public PlayerStatInt ShieldDuration { get; private set; }
    public PlayerStatFloat MaxHealth { get; private set; }
    public PlayerStatFloat Damage { get; private set; }
    public PlayerStatFloat BleedDamage { get; private set; }
    public PlayerStatFloat BleedDuration { get; private set; }
    public PlayerStatFloat PoisonDamage { get; private set; }
    public PlayerStatFloat PoisonDuration { get; private set; }
    public PlayerStatFloat BurnDamage { get; private set; }
    public PlayerStatFloat BurnDuration { get; private set; }
    public PlayerStatFloat CritChance { get; private set; }
    public PlayerStatFloat CritDamage { get; private set; }
    public PlayerStatFloat Armor { get; private set; }
    public PlayerStatFloat Lifesteal { get; private set; }
    public PlayerStatInt Gold { get; private set; }

    // Bonus damage for next ability (set by StalkingPounce)
    public float NextAbilityDamageBonus { get; set; }

    // Progression
    public PlayerStatInt Level { get; private set; }
    public PlayerStatInt CurrentXP { get; private set; }

    // XP requirements table keyed by target level (e.g., 2 => XP needed from L1->L2)
    private static readonly System.Collections.Generic.Dictionary<int, int> XpTable = new()
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
        MaxHealth = new PlayerStatFloat(20);
        Health = new PlayerStatFloat(MaxHealth.Value).OnMin(_ => GameManager.Instance.RestartGame());
        Shield = new PlayerStatFloat(0);
        ShieldDuration = new PlayerStatInt(0);
        Gold = new PlayerStatInt(110);
        Damage = new PlayerStatFloat(0);
        BleedDamage = new PlayerStatFloat(4);
        BleedDuration = new PlayerStatFloat(3);
        PoisonDamage = new PlayerStatFloat(2);
        PoisonDuration = new PlayerStatFloat(10);
        BurnDamage = new PlayerStatFloat(3);
        BurnDuration = new PlayerStatFloat(5);
        CritChance = new PlayerStatFloat(10);
        CritDamage = new PlayerStatFloat(150f);
        Armor = new PlayerStatFloat(0);
        Lifesteal = new PlayerStatFloat(0);

        Level = new PlayerStatInt(1);
        CurrentXP = new PlayerStatInt(0);

        // React to changes for UI updates
        Level.OnValueChanged += OnLevelValueChanged;
        CurrentXP.OnValueChanged += OnXPValueChanged;

        AbilitiesInLoop =
        [
            new PlayerAbilityResource(GlobalManager.Abilities[AbilityName.IronFang]),
            new PlayerAbilityResource(GlobalManager.Abilities[AbilityName.Pounce]),
            new PlayerAbilityResource(GlobalManager.Abilities[AbilityName.StalkingPounce]),
            new PlayerAbilityResource(GlobalManager.Abilities[AbilityName.InfernoKick]),
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
        Health.SetMax(MaxHealth.Value);
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

    public void AddShield(float amount)
    {
        if (Shield.Value == 0)
        {
            GlobalManager.Player.StartShieldDuration();
        }
        Shield.Add(amount);
    }
    
    public int GetTraitCount(AbilityTrait trait)
    {
        return AbilitiesInLoop.Count(a => a.AbilityResource.Traits.Contains(trait));
    }

    public void IncreaseDamage(float amount) => Damage.Add(amount);

    public void AddAbility(AbilityResource abilityResource)
    {
        TryAddAbility(abilityResource);
    }

    public bool TryAddAbility(AbilityResource abilityResource)
    {
        var currentAbility = AbilitiesInLoop.ToList()
            .FirstOrDefault(a => a.AbilityResource.AbilityName == abilityResource.AbilityName);

        if (currentAbility == null)
        {
            // Enforce capacity: number of distinct abilities cannot exceed Level
            if (AbilitiesInLoop.Count >= Level.Value + 1)
            {
                // Not enough capacity to add a new ability
                GD.Print("Ability loop is full for current level. Level up to add more abilities.");
                return false;
            }
            AbilitiesInLoop.Add(new PlayerAbilityResource(abilityResource));
        }
        else
        {
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
        if (level >= 10) return 0; // at cap, no further XP required
        return XpTable.GetValueOrDefault(level + 1, 0);
    }

    public int SpendGoldForXp(int goldToSpend, int rateGoldPerXp)
    {
        var xp = goldToSpend * rateGoldPerXp;
        if (xp <= 0) return 0;

        Gold.Subtract(goldToSpend);
        AddXP(xp);
        return xp;
    }

    // Reorder abilities within the loop
    public bool MoveAbility(int fromIndex, int toIndex)
    {
        var count = AbilitiesInLoop.Count;
        if (fromIndex < 0 || fromIndex >= count) return false;
        if (toIndex < 0 || toIndex >= count) return false;
        if (fromIndex == toIndex) return false;

        // Convert to list for easier manipulation
        var list = AbilitiesInLoop.ToList();
        var item = list[fromIndex];
        list.RemoveAt(fromIndex);
        list.Insert(toIndex, item);

        // Write back to Godot Array
        AbilitiesInLoop = new Array<PlayerAbilityResource>(list);
        EmitSignal(SignalName.OnAbilitiesChanged, AbilitiesInLoop);
        return true;
    }

    public bool SwapAbilities(int indexA, int indexB)
    {
        var count = AbilitiesInLoop.Count;
        if (indexA < 0 || indexA >= count) return false;
        if (indexB < 0 || indexB >= count) return false;
        if (indexA == indexB) return false;

        var list = AbilitiesInLoop.ToList();
        (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
        AbilitiesInLoop = new Array<PlayerAbilityResource>(list);
        EmitSignal(SignalName.OnAbilitiesChanged, AbilitiesInLoop);
        return true;
    }
    
    public int GetSellValueForAbility(int index)
    {
        if (index < 0 || index >= AbilitiesInLoop.Count) return 0;

        var ability = AbilitiesInLoop[index];
        var price = ability.AbilityResource.Price;
        var sellValue = ability.Level switch
        {
            1 => ability.Copies * price,
            2 => price * 4 + ability.Copies * price,
            3 => price * 10, 
        };
        int baseValue = ability.AbilityResource.Price * ability.Level;

        // Common sells for full value always
        if (ability.AbilityResource.Rarity == AbilityRarity.Common)
            return baseValue;

        // Non-common depreciation by level
        float mult = ability.Level switch
        {
            1 => 1f,          
            2 => 2f / 3f,
            3 => 1f / 3f,
        };

        return Mathf.RoundToInt(baseValue * mult);
    }

    public void SellAbility(int index)
    {
        int sellValue = GetSellValueForAbility(index);
        if (sellValue <= 0) return;

        Gold.Add(sellValue);
        AbilitiesInLoop.RemoveAt(index);
        EmitSignal(SignalName.OnAbilitiesChanged, AbilitiesInLoop);
    }
}