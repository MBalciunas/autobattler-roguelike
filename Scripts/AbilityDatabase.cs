using System.Text.Json.Serialization;
using Godot.Collections;

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

        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };

        var raw = JsonSerializer.Deserialize<Dictionary<string, AbilityJson>>(jsonText, options);

        foreach (var kv in raw)
        {
            var key = kv.Key;
            var data = kv.Value;

            var abilityName = Enum.Parse<AbilityName>(key);
            var iconPath = GetIconPath(key);
            var scenePath = GetScenePath(key, data.rarity.ToString());

            AllAbilities[abilityName] = new AbilityResource
            {
                AbilityName = abilityName,
                Name = data.name,
                Description = data.description,
                Rarity = data.rarity, 
                Price = data.price,
                Icon = GD.Load<Texture2D>(iconPath),
                AbilityScene = GD.Load<PackedScene>(scenePath),
                Traits = new Array<AbilityTrait>(data.traits)
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

    public AbilityRarity rarity { get; set; }

    public int price { get; set; }

    public List<AbilityTrait> traits { get; set; }
}