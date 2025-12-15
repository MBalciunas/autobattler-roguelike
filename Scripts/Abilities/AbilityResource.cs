using Godot;
using System;
using Godot.Collections;

public partial class AbilityResource : Resource
{
    [Export] public String name;
    [Export] public String description;
    [Export] public AbilityRarity rarity;
    [Export] public int price;
    [Export] public Texture2D icon;
    [Export] public PackedScene abilityScene;
    [Export] public AbilityName abilityName;
    [Export] public Array<AbilityTrait> Traits;
}

public enum AbilityRarity
{
    Common, Uncommon, Rare, Epic, Legendary
}

public enum AbilityName
{
    ToxicDart
}

public enum AbilityTrait
{
    WitchDoctor, Butcher, Stalker, Huntsman, Warden, Arcanist
}

public enum DamageType
{
    Poison, Physical, Fire, Bleed
}