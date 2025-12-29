using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class CleaveEffect : Area2D
{
    private float damage;
    private float bleedDamage;
    private float bleedDuration;

    private void OnAreaEntered(Area2D area)
    {
        if (area is Enemy enemy)
        {
            enemy.TakeDamage(damage);
            enemy.AddActiveDot(new DamageOverTime(bleedDamage, bleedDuration, ElementType.Bleed));
        }
    }

    public void Init((float cleaveDamage, float bleedDamage, int bleedDuration) stats)
    {
        damage = stats.cleaveDamage;
        bleedDamage = stats.bleedDamage;
        bleedDuration = stats.bleedDuration;
    }
}