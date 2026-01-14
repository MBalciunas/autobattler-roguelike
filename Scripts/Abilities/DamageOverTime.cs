namespace AutoBattlerRoguelike.Scripts.Abilities;

public class DamageOverTime(float damage, float duration, ElementType elementType, int stacks = 1)
{
    public float damage = damage;
    public float durationLeft = duration;
    public ElementType elementType = elementType;
    public int stacks = stacks;

    public static DamageOverTime GetBleed(int stacks)
    {
        return new DamageOverTime(
            GlobalManager.playerState.BleedDamage.Value,
            GlobalManager.playerState.BleedDuration.Value,
            ElementType.Bleed,
            stacks
        );
    }
    
    public static DamageOverTime GetPoison(int stacks)
    {
        return new DamageOverTime(
            GlobalManager.playerState.PoisonDamage.Value,
            GlobalManager.playerState.PoisonDuration.Value,
            ElementType.Poison,
            stacks
        );
    }

    public void ResetDuration()
    {
        durationLeft = elementType switch
        {
            ElementType.Poison => GlobalManager.playerState.PoisonDuration.Value,
            ElementType.Bleed => GlobalManager.playerState.BleedDuration.Value,
            ElementType.Fire => durationLeft
        };
    }
}