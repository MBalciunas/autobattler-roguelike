using System.Text.Json.Serialization;
using Godot.Collections;

namespace AutoBattlerRoguelike.Scripts;

using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

public static class TraitDatabase
{

    public static Godot.Collections.Dictionary<AbilityTrait, TraitData> Load()
    {
        var allTraits = new Godot.Collections.Dictionary<AbilityTrait, TraitData>();

        using var file = FileAccess.Open("res://Data/Traits.json", FileAccess.ModeFlags.Read);
        var jsonText = file.GetAsText();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var raw = JsonSerializer.Deserialize<Dictionary<string, TraitJson>>(jsonText, options);

        foreach (var kv in raw)
        {
            var key = kv.Key;
            var data = kv.Value;

            var traitEnum = Enum.Parse<AbilityTrait>(key);

            var tiers = new List<TraitTier>();
            foreach (var tierJson in data.tiers)
            {
                tiers.Add(new TraitTier
                {
                    Required = tierJson.required,
                    Effect = tierJson.effect
                });
            }

            allTraits[traitEnum] = new TraitData
            {
                Name = data.name,
                Description = data.description,
                Tiers = tiers
            };
        }

        return allTraits;
    }
}
