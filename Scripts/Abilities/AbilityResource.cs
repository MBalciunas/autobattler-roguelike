using System.Collections.Generic;
using System.Text;
using AutoBattlerRoguelike.Scripts.Abilities;
using Godot;
using Godot.Collections;

public partial class AbilityResource : Resource
{
    [Export] public string Name;
    [Export] public string Description;
    [Export] public AbilityRarity Rarity;
    [Export] public int Price;
    [Export] public Texture2D Icon;
    [Export] public PackedScene AbilityScene;
    [Export] public AbilityName AbilityName;
    [Export] public Array<AbilityTrait> Traits;

    public System.Collections.Generic.Dictionary<string, AbilityLevelStats> Stats { get; set; } = new();
    public System.Collections.Generic.Dictionary<string, string> LevelBonuses { get; set; } = new();

    public AbilityLevelStats GetStats(int level)
    {
        return Stats.TryGetValue(level.ToString(), out var stats) ? stats : new AbilityLevelStats();
    }

    public string GetLevelDescription(int level)
    {
        var stats = GetStats(level);
        var parts = new List<string>();

        if (stats.damage > 0) parts.Add($"{stats.damage} dmg");
        if (stats.bleedStacks > 0) parts.Add($"{stats.bleedStacks} bleed");
        if (stats.poisonStacks > 0) parts.Add($"{stats.poisonStacks} poison");
        if (stats.burnStacks > 0) parts.Add($"{stats.burnStacks} burn");
        if (stats.shield > 0) parts.Add($"{stats.shield} shield");
        if (stats.shieldPerEnemy > 0) parts.Add($"{stats.shieldPerEnemy} shield/enemy");
        if (stats.slow > 0) parts.Add($"{(int)(stats.slow * 100)}% slow");
        if (stats.slowDuration > 0) parts.Add($"{stats.slowDuration}s");
        if (stats.duration > 0 && stats.slow == 0) parts.Add($"{stats.duration}s duration");
        if (stats.knockbackStrength > 0) parts.Add("knockback");
        if (stats.explosionRadius > 0) parts.Add($"{stats.explosionRadius} radius");
        if (stats.poisonedDamage > 0) parts.Add($"{stats.poisonedDamage} if poisoned");
        if (stats.projectileCount > 1) parts.Add($"{stats.projectileCount} projectiles");
        if (stats.goldOnKillChance > 0) parts.Add($"{(int)(stats.goldOnKillChance * 100)}% +1 gold on kill");
        if (stats.ricochets > 0) parts.Add($"{stats.ricochets} ricochets");

        var desc = string.Join(", ", parts);

        if (LevelBonuses.TryGetValue(level.ToString(), out var bonus))
        {
            desc += (desc.Length > 0 ? ". " : "") + bonus;
        }

        return desc;
    }
}

public enum AbilityRarity
{
    Common, Uncommon, Rare, Epic, Legendary
}

public enum AbilityName
{
    ToxicDart, ShadowStrike, Cleave, Stomp, FieryCharge, EmberSpit, QuickJab, CoinToss, HeavySlam, Peck, // Common
    VenomFang, IronWing, DragonBreath, Tremor, PhoenixDive, // Uncommon
    CrimsonSpike, Pounce, SerpentDance, DragonTail, ToxicPool // Rare
}

public enum AbilityTrait
{
    Serpent, Dragon, Crane, Tiger, Ox, Bear, Mantis, Phoenix, Monkey, Leopard
}

public enum ElementType
{
    Poison, Fire, Bleed, None
}

public enum DebuffType
{
    Slow, Stun
}

public enum DamageType
{
    DoT, Direct
}