using Godot;
using Godot.Collections;

namespace AutoBattlerRoguelike.Scripts;

[GlobalClass]
public partial class PlayerState : Resource
{
    public PlayerStatFloat Health { get; private set; }
    public PlayerStatFloat MaxHealth { get; private set; }
    public PlayerStatFloat Damage { get; private set; }
    public PlayerStatInt Gold { get; private set; }
    public Array<AbilityResource> AbilitiesInLoop { get; private set; }
    
    [Signal]
    public delegate void OnAbilitiesChangedEventHandler(Array<AbilityResource> AbilitiesInLoop);
    

    public PlayerState()
    {
        InitializeStats();
    }

    public void InitializeStats()
    {
        MaxHealth = new PlayerStatFloat(10);
        Health = new PlayerStatFloat(10).OnMin(_ => GameManager.Instance.ReloadLevel());
        Health.SetMax(MaxHealth.Value);
        Gold = new PlayerStatInt(0);
        Damage = new PlayerStatFloat(0);
        
        AbilitiesInLoop =
        [
            GlobalManager.Abilities[AbilityName.Firebolt],
            GlobalManager.Abilities[AbilityName.Icicle],
        ];
    }

    public void ResetHealth()
    {
        Health.Value = MaxHealth.Value;
    }

    public void Heal(float amount) => Health.Add(amount);

    public void TakeDamage(float amount) { Health.Add(-amount); }
    
    public void AddGold(int amount) {  Gold.Add(amount); }

    public void IncreaseDamage(float amount) => Damage.Add(amount);

    public void AddAbility(AbilityResource abilityResource)
    {
        AbilitiesInLoop.Add(abilityResource);
        EmitSignal(SignalName.OnAbilitiesChanged, AbilitiesInLoop);
    }
}