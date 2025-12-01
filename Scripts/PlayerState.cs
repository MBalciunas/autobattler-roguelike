using Godot;

namespace AutoBattlerRoguelike.Scripts;

[GlobalClass]
public partial class PlayerState : Resource
{
    public PlayerStatFloat Health { get; private set; }
    public PlayerStatFloat MaxHealth { get; private set; }
    public PlayerStatFloat Damage { get; private set; }

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
    }

    public void SetHealthWithMaxHealthIncrease(int health)
    {
        if (health > MaxHealth.Value)
        {
            MaxHealth.Value = health;
        }
        Health.Value = health;
    }

    public void Reset()
    {
        MaxHealth.Value = 10;
        Health.Value = 10;
        Damage.Value = 0;
    }

    public void Heal(float amount) => Health.Add(amount);

    public void TakeDamage(float amount) { Health.Add(-amount); }

    public void IncreaseDamage(float amount) => Damage.Add(amount);
}