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

    public void RestartGame()
    {
        playerState.InitializeStats();
        Level = 1;
        GetTree().ChangeSceneToFile("res://Scenes/main_level.tscn");
    }

    public void FinishLevel()
    {
        // 1) Apply gold interest BEFORE base after-round gold
        var interest = Mathf.Min(playerState.Gold.Value / 10, 5); // +1 per full 10 gold, up to +5
        if (interest > 0)
        {
            playerState.AddGold(interest);
        }

        // 2) Base after-round gold
        playerState.AddGold(5);

        // 3) Per-round XP: base 4, +1 every 5 rounds, first +1 at round 6
        var roundIndex = Level; // assuming one round per level
        var roundXp = 4 + (roundIndex - 1) / 5;
        playerState.AddXP(roundXp);

        // 4) Proceed as before
        playerState.ResetHealth();
        GetTree().CallDeferred("change_scene_to_file", "res://Scenes/shop_scene.tscn");
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