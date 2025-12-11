using Godot;

public partial class GoldUI : Label
{
    public override void _Ready()
    {
        Text = "Gold: " + GlobalManager.playerState.Gold.Value;
        GlobalManager.playerState.Gold.OnValueChanged += UpdateUI;
    }

    private void UpdateUI(int value)
    {
        Text = "Gold: " + value;
    }

    public override void _ExitTree()
    {
        GlobalManager.playerState.Gold.OnValueChanged -= UpdateUI;
    }
}