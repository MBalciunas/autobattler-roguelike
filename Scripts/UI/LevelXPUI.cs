using Godot;

public partial class LevelXPUI : Label
{
    public override void _Ready()
    {
        // Initialize and subscribe
        UpdateText(GlobalManager.playerState.Level.Value, GlobalManager.playerState.CurrentXP.Value, GlobalManager.playerState.GetXpToNextLevel());
        GlobalManager.playerState.Level.OnValueChanged += OnLevelChanged;
        GlobalManager.playerState.CurrentXP.OnValueChanged += OnXPChanged;
    }

    private void OnLevelChanged(int level)
    {
        UpdateText(level, GlobalManager.playerState.CurrentXP.Value, GlobalManager.playerState.GetXpToNextLevel());
    }

    private void OnXPChanged(int currentXP)
    {
        UpdateText(GlobalManager.playerState.Level.Value, currentXP, GlobalManager.playerState.GetXpToNextLevel());
    }

    private void UpdateText(int level, int currentXP, int toNext)
    {
        if (level >= 10)
        {
            Text = $"Level: 10  |  XP to next: MAX ( {currentXP}/— )";
        }
        else
        {
            Text = $"Level: {level}  |  XP to next: {toNext} ( {currentXP}/{GlobalManager.playerState.GetXpRequired(level)} )";
        }
    }

    public override void _ExitTree()
    {
        GlobalManager.playerState.Level.OnValueChanged -= OnLevelChanged;
        GlobalManager.playerState.CurrentXP.OnValueChanged -= OnXPChanged;
    }
}
