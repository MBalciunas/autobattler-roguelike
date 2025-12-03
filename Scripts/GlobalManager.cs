using AutoBattlerRoguelike.Scripts;
using Godot;
using Godot.Collections;


[GlobalClass]
public partial class GlobalManager : Node
{
    public static PlayerState playerState;

    public static Player Player;

    public override void _EnterTree()
    {
        playerState = ResourceLoader.Load<PlayerState>("res://Resources/PlayerState.tres");
        GD.Print("initialize stats");
        playerState.InitializeStats(); 
    }
    
    public static Dictionary<AbilityName, AbilityResource> Abilities = new()
    {
        { AbilityName.Firebolt, ResourceLoader.Load<AbilityResource>("res://Resources/Abilities/Common/FireboltAbility.tres") },
        { AbilityName.Icicle, ResourceLoader.Load<AbilityResource>("res://Resources/Abilities/Common/IcicleAbility.tres") },
    };
    
    public void ReloadLevel()
    {
        playerState.InitializeStats();
        GetTree().ChangeSceneToFile("res://Scenes/main_level.tscn");
    } 
}

public enum AbilityName
{
    Firebolt, Icicle
}