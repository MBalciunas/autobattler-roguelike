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

    private static Dictionary<int, int> StatAmountPerLevel = new()
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

    private static Dictionary<UpgradablePlayerStat, float> StatPerGenericStatValue = new()
    {
        { UpgradablePlayerStat.MaxHealth, 1 },
        { UpgradablePlayerStat.Armor, 1 },
        { UpgradablePlayerStat.CritChance, 0.2f },
        { UpgradablePlayerStat.CritDamage, 1 },
        { UpgradablePlayerStat.Damage, 1 },
        { UpgradablePlayerStat.Lifesteal, 0.2f },
    };


    public override void _EnterTree()
    {
        playerState = ResourceLoader.Load<PlayerState>("res://Resources/PlayerState.tres");
        playerState.InitializeStats();
    }

    public static Godot.Collections.Dictionary<AbilityName, AbilityResource> Abilities = AbilityDatabase.Load();

    public void RestartGame()
    {
        playerState.InitializeStats();
        Level = 1;
        GetTree().ChangeSceneToFile("res://Scenes/main_level.tscn");
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

        playerState.ResetHealth();
        GetTree().CallDeferred("change_scene_to_file", "res://Scenes/shop_scene.tscn");
    }

    public static int CalculateInterest()
    {
        return Mathf.Min(playerState.Gold.Value / 10, 5);
    }
    
    public static int CalculateGoldAfterRound()
    {
        return 5 + Level / 5 + 1;
    }

    public static int CalculateXpGain()
    {
        return 4 + (Level - 1) / 5;
    }

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
            dict[selectedStat] = dict.GetValueOrDefault(selectedStat) + StatPerGenericStatValue[selectedStat];
            statAmount--;
        }

        return dict;
    }

    public void LoadNextLevel()
    {
        Level++;
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
}