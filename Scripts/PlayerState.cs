using System.Linq;
using Godot;
using Godot.Collections;

namespace AutoBattlerRoguelike.Scripts;

[GlobalClass]
public partial class PlayerState : Resource
{
    public PlayerStatFloat Health { get; private set; }
    public PlayerStatFloat MaxHealth { get; private set; }
    public PlayerStatFloat Damage { get; private set; }
    public PlayerStatFloat CritChance { get; private set; }
    public PlayerStatFloat CritDamage { get; private set; }
    public PlayerStatFloat Armor { get; private set; }
    public PlayerStatFloat Lifesteal { get; private set; }
    public PlayerStatInt Gold { get; private set; }
    public Array<PlayerAbilityResource> AbilitiesInLoop { get; private set; }

    [Signal]
    public delegate void OnAbilitiesChangedEventHandler(Array<PlayerAbilityResource> AbilitiesInLoop);


    public PlayerState()
    {
        InitializeStats();
    }

    public void InitializeStats()
    {
        MaxHealth = new PlayerStatFloat(10);
        Health = new PlayerStatFloat(10).OnMin(_ => GameManager.Instance.RestartGame());
        Health.SetMax(MaxHealth.Value);
        Gold = new PlayerStatInt(0);
        Damage = new PlayerStatFloat(0);
        CritChance = new PlayerStatFloat(10);
        CritDamage = new PlayerStatFloat(1.5f);
        Armor = new PlayerStatFloat(0);
        Lifesteal = new PlayerStatFloat(0);

        AbilitiesInLoop =
        [
            new PlayerAbilityResource(GlobalManager.Abilities[AbilityName.ToxicDart]),
        ];
    }

    public void ResetHealth()
    {
        Health.Value = MaxHealth.Value;
    }

    public void Heal(float amount) => Health.Add(amount);

    public void TakeDamage(float amount)
    {
        Health.Add(-amount);
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
        var currentAbility = AbilitiesInLoop.ToList()
            .FirstOrDefault(a => a.AbilityResource.AbilityName == abilityResource.AbilityName);

        if (currentAbility == null)
        {
            AbilitiesInLoop.Add(new PlayerAbilityResource(abilityResource));
        }
        else
        {
            currentAbility.AddCopy();
        }

        EmitSignal(SignalName.OnAbilitiesChanged, AbilitiesInLoop);
    }
}