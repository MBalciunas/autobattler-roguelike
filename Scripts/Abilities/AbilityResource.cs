using Godot;
using Godot.Collections;

public partial class AbilityResource : Resource
{
    [Export] public string Name;
    [Export] public string Description;
    [Export] public string Level1Effect;
    [Export] public string Level2Effect;
    [Export] public string Level3Effect;
    [Export] public AbilityRarity Rarity;
    [Export] public int Price;
    [Export] public Texture2D Icon;
    [Export] public PackedScene AbilityScene;
    [Export] public AbilityName AbilityName;
    [Export] public Array<AbilityTrait> Traits;
}

public enum AbilityRarity
{
    Common, Uncommon, Rare, Epic, Legendary
}

public enum AbilityName
{
    ToxicDart, ShadowStrike, Cleave, Stomp, SteelPlume, // Common
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