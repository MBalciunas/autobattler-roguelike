using Godot;


public partial class GameManager : Node
{
    private GlobalManager globalManager;
    public static GameManager Instance;
    
    public override void _Ready()
    {
        Instance = this;
        globalManager = GetNode<GlobalManager>("/root/Global");
        GlobalManager.Player = GetTree().Root.GetNode<Player>("MainLevel/Player");
    }
    
    public void ReloadLevel()
    {
        globalManager.ReloadLevel();
    } 
}