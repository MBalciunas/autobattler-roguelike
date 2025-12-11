namespace AutoBattlerRoguelike.Scripts;

using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

public static class AbilityDatabase
{
    public static Godot.Collections.Dictionary<AbilityName, AbilityResource> AllAbilities { get; private set; }

    public static Godot.Collections.Dictionary<AbilityName, AbilityResource> Load()
    {
        AllAbilities = new Godot.Collections.Dictionary<AbilityName, AbilityResource>();

        using var file = FileAccess.Open("res://Data/Abilities.json", FileAccess.ModeFlags.Read);
        var jsonText = file.GetAsText();

        var raw = JsonSerializer.Deserialize<Dictionary<string, AbilityJson>>(jsonText);

        foreach (var kv in raw)
        {
            var key = kv.Key;
            var data = kv.Value;

            var abilityName = Enum.Parse<AbilityName>(key);

            var id = key; 

            var iconPath = GetIconPath(id);
            var scenePath = GetScenePath(id, data.rarity);

            AllAbilities[abilityName] = new AbilityResource
            {
                abilityName = abilityName,
                name = data.name,
                description = data.description,
                rarity = Enum.Parse<AbilityRarity>(data.rarity),
                price = data.price,
                icon = GD.Load<Texture2D>(iconPath),
                abilityScene = GD.Load<PackedScene>(scenePath),
            };
        }

        return AllAbilities;
    }

    private static string GetIconPath(string id)
        => $"res://Art/{id}.svg";

    private static string GetScenePath(string id, string rarity)
        => $"res://Scenes/Abilities/{rarity}/{id}Ability.tscn";
}

public class AbilityJson
{
    public string name { get; set; }
    public string description { get; set; }
    public string rarity { get; set; }
    public int price { get; set; }
}
