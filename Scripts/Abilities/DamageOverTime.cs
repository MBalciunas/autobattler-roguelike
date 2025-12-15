namespace AutoBattlerRoguelike.Scripts.Abilities;

public class DamageOverTime(float damage, float duration, DamageType damageType)
{
    public float damage = damage;
    public float durationLeft = duration;
    public DamageType damageType = damageType;
}