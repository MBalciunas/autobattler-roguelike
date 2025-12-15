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

    public override void _EnterTree()
    {
        playerState = ResourceLoader.Load<PlayerState>("res://Resources/PlayerState.tres");
        playerState.InitializeStats();
    }

    public static Godot.Collections.Dictionary<AbilityName, AbilityResource> Abilities = AbilityDatabase.Load();

    public void ReloadLevel()
    {
        playerState.InitializeStats();
        GetTree().ChangeSceneToFile("res://Scenes/main_level.tscn");
    }

    public void FinishLevel()
    {
        playerState.AddGold(5);
        playerState.ResetHealth();
        GetTree().CallDeferred("change_scene_to_file", "res://Scenes/shop_scene.tscn");
    }

    public void LoadNextLevel()
    {
        Level++;
        GD.Print("Loading level " + Level);
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