using System.Collections.Generic;
using Godot;

namespace AutoBattlerRoguelike.Scripts;

[GlobalClass]
public partial class PlayerState : Resource
{
    public PlayerStatFloat Health { get; private set; }
    public PlayerStatFloat MaxHealth { get; private set; }
    public PlayerStatFloat Damage { get; private set; }
    public List<AbilityResource> AbilitiesInLoop { get; set; }
    

    public PlayerState()
    {
        InitializeStats();
    }

    public void InitializeStats()
    {
        MaxHealth = new PlayerStatFloat(10);
        Health = new PlayerStatFloat(10).OnMin(_ => GameManager.Instance.ReloadLevel());
        Health.SetMax(MaxHealth.Value);
        
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

    public void IncreaseDamage(float amount) => Damage.Add(amount);
}