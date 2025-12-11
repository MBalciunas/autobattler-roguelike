using Godot;
using System;

public partial class AbilityResource : Resource
{
    [Export] public String name;
    [Export] public String description;
    [Export] public AbilityRarity rarity;
    [Export] public int price;
    [Export] public Texture2D icon;
    [Export] public PackedScene abilityScene;
    [Export] public AbilityName abilityName;
}

public enum AbilityRarity
{
    Common, Uncommon, Rare, Epic, Legendary
}

public enum AbilityName
{
    Firebolt, Icicle
}