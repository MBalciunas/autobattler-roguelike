using Godot;
using System;

public partial class AbilityResource : Resource
{
    [Export] String name;
    [Export] String description;
    [Export] AbilityRarity rarity;
    [Export] int price;
    [Export] Texture2D icon;
}

enum AbilityRarity
{
    Common, Uncommon, Rare, Epic, Legendary
}
