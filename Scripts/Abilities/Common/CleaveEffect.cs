using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class CleaveEffect : Area2D
{
    private float damage;
    private int bleedStacks;

    private void OnAreaEntered(Area2D area)
    {
        if (area is Enemy enemy)
        {
            enemy.TakeDamage(damage);
            enemy.AddActiveDot(DamageOverTime.GetBleed(bleedStacks));
        }
    }

    public void Init((float damage, int bleedStacks) stats)
    {
        this.damage = stats.damage;
        this.bleedStacks = stats.bleedStacks;
    }
}