using Godot;


public partial class GameManager : Node
{
    private GlobalManager globalManager;
    [Export] private RoundEndSelection roundEndScene;
    public static GameManager Instance;
    
    public override void _Ready()
    {
        Instance = this;
        globalManager = GetNode<GlobalManager>("/root/Global");
        GlobalManager.Player = GetTree().Root.GetNode<Player>("MainLevel/Player");
    }
    
    public void RestartGame()
    {
        globalManager.RestartGame();
    } 
    
    public void FinishLevel()
    {
        roundEndScene.Show();
        roundEndScene.Update();
    } 
    
    public void StatsChosen()
    {
        globalManager.FinishLevel();
    } 
    
    public void LoadNextLevel()
    {
        globalManager.LoadNextLevel();
    } 
}