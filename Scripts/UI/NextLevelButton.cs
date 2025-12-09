using Godot;

public partial class NextLevelButton : Button
{
    public override void _Ready()
    {
        Pressed += () => GameManager.Instance.LoadNextLevel();
    }
}
