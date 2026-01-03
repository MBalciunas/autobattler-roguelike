using Godot;
using System;
using Godot.Collections;

public partial class AbilityResource : Resource
{
    [Export] public String Name;
    [Export] public String Description;
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
    ToxicDart, ShadowStep, Cleave, Stomp, FieryCharge, // Common
    VenomFang, IronWing // Uncommon
}

public enum AbilityTrait
{
    Serpent, Dragon, Crane, Tiger, Ox, Bear, Mantis, Eagle, Monkey, Leopard
}

public enum ElementType
{
    Poison, Fire, Bleed
}

public enum DamageType
{
    DoT, Direct
}