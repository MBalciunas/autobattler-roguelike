namespace AutoBattlerRoguelike.Scripts.Abilities;

public class DamageOverTime(float damage, float duration, ElementType damageType)
{
    public float damage = damage;
    public float durationLeft = duration;
    public ElementType damageType = damageType;
}