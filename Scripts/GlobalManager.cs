using AutoBattlerRoguelike.Scripts;
using Godot;


[GlobalClass]
public partial class GlobalManager : Node
{
    public static PlayerState playerState = ResourceLoader.Load<PlayerState>("res://Resources/PlayerState.tres");

    public void ReloadLevel()
    {
        playerState.InitializeStats();
        GetTree().ChangeSceneToFile("res://Scenes/main_level.tscn");
    } 
}