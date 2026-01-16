using System;
using System.Collections.Generic;
using System.Linq;
using AutoBattlerRoguelike.Scripts;
using Godot;

[GlobalClass]
public partial class GlobalManager : Node
{
    public static PlayerState playerState;

    public static Player Player;

    public static int Level = 1;
    public static bool IsEnemiesSpawning;

    private static Godot.Collections.Dictionary<int, int> StatAmountPerLevel = new()
    {
        { 1, 5 },
        { 2, 6 },
        { 3, 7 },
        { 4, 8 },
        { 5, 20 },
        { 6, 10 },
        { 7, 12 },
        { 8, 14 },
        { 9, 16 },
        { 10, 30 },
        { 11, 20 },
        { 12, 22 },
        { 13, 24 },
        { 14, 26 },
        { 15, 40 },
        { 16, 44 },
        { 17, 48 },
    };


    // TODO after Epic and Legendary abilities will be added
    private static List<float> GetRarityChancesForLevel(int level)
    {
        level = Mathf.Min(playerState.Level.Value, 10);

        return level switch
        {
            1 or 2 => [100, 0, 0, 0, 0],
            3 => [75, 25, 0, 0, 0],
            4 => [55, 30, 15, 0, 0],
            5 => [45, 33, 22, 0, 0],
            6 => [30, 45, 25, 0, 0],
            7 => [24, 37, 39, 0, 0],
            8 => [16, 30, 54, 0, 0],
            9 => [12, 26, 62, 0, 0],
            10 => [10, 25, 65, 0, 0],
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static Godot.Collections.Dictionary<UpgradablePlayerStat, float> StatPerGenericStatValue = new()
    {
        { UpgradablePlayerStat.MaxHealth, 1 },
        { UpgradablePlayerStat.Armor, 1 },
        { UpgradablePlayerStat.CritChance, 0.2f },
        { UpgradablePlayerStat.CritDamage, 1 },
        { UpgradablePlayerStat.Damage, 1 },
        { UpgradablePlayerStat.Lifesteal, 0.2f },
        { UpgradablePlayerStat.PoisonDamage, 0.2f },
        { UpgradablePlayerStat.BleedDamage, 0.2f },
    };


    public override void _EnterTree()
    {
        playerState = ResourceLoader.Load<PlayerState>("res://Resources/PlayerState.tres");
        playerState.InitializeStats();
    }

    public static Godot.Collections.Dictionary<AbilityName, AbilityResource> Abilities = AbilityDatabase.Load();

    private static AbilityRarity RollRarity()
    {
        var chances = GetRarityChancesForLevel(Level);
        var roll = GD.Randf() * 100f;
        float cumulative = 0f;

        for (int i = 0; i < chances.Count; i++)
        {
            cumulative += chances[i];
            if (roll < cumulative)
            {
                return (AbilityRarity)i;
            }
        }

        // safety fallback
        AbilityRarity abilityRarity = (AbilityRarity)(chances.Count - 1);
        return abilityRarity;
    }

    private static AbilityResource PickRandomAbility(IEnumerable<AbilityResource> source)
    {
        int count = 0;
        AbilityResource selected = null;

        foreach (var item in source)
        {
            count++;
            if (GD.Randi() % count == 0)
                selected = item;
        }

        return selected;
    }

    public static AbilityResource RollAbility()
    {
        var rolledRarity = RollRarity();
        var picked = PickRandomAbility(
            Abilities.Values.Where(a => a.Rarity == rolledRarity)
        );

        return picked;
    }

    public void RestartGame()
    {
        playerState.InitializeStats();
        Level = 1;
        GetTree().ChangeSceneToFile("res://Scenes/main_level.tscn");
    }

    public static int CalculateInterest() => Mathf.Min(playerState.Gold.Value / 5, 10);

    public static int CalculateGoldAfterRound() => 5 + Level / 5 * 3;

    public static int CalculateXpGain() => 4 + (Level - 1) / 5;

    public static Dictionary<UpgradablePlayerStat, float> RollRandomStatsSelection()
    {
        var statAmount = StatAmountPerLevel[Level];
        var differentStats = GD.RandRange(1, 2);

        var stats = Enum.GetValues<UpgradablePlayerStat>().OrderBy(_ => Guid.NewGuid()).Take(differentStats).ToList();
        var dict = new Dictionary<UpgradablePlayerStat, float>();

        while (statAmount > 0)
        {
            int randRange = GD.RandRange(0, differentStats - 1);
            var selectedStat = stats[randRange];
            if (dict.TryGetValue(selectedStat, out float value))
            {
                dict[selectedStat] = value + StatPerGenericStatValue[selectedStat];
            }
            else
            {
                dict[selectedStat] = StatPerGenericStatValue[selectedStat];
            }

            statAmount--;
        }

        return dict;
    }

    public void FinishLevel()
    {
        var interest = CalculateInterest();
        if (interest > 0)
        {
            playerState.AddGold(interest);
        }

        playerState.AddGold(CalculateGoldAfterRound());


        playerState.AddXP(CalculateXpGain());

        GetTree().CallDeferred("change_scene_to_file", "res://Scenes/shop_scene.tscn");
    }

    public void LoadNextLevel()
    {
        Level++;
        playerState.ResetHealth();
        GetTree().CallDeferred("change_scene_to_file", "res://Scenes/main_level.tscn");
    }

    public static List<Enemy> GetEnemiesSortedByClosest()
    {
        return Player.GetTree().GetNodesInGroup("Enemies")
            .Cast<Enemy>()
            .OrderBy(e => e.GlobalPosition.DistanceTo(Player.GlobalPosition))
            .ToList();
    }
}

public enum UpgradablePlayerStat
{
    MaxHealth,
    Damage,
    Armor,
    CritChance,
    CritDamage,
    Lifesteal,
    PoisonDamage,
    BleedDamage
}