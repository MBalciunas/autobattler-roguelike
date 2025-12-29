using System.Globalization;
using Godot;

public partial class HealthUI : Control
{
    private Label Amount;
    private TextureProgressBar HealthBar;
    public override void _Ready()
    {
        Amount = GetNode<Label>("Amount");
        HealthBar = GetNode<TextureProgressBar>("Bar");
        Amount.Text = GlobalManager.playerState.Health.Value.ToString(CultureInfo.InvariantCulture);
        GlobalManager.playerState.Health.OnValueChanged += UpdateUI;
    }

    private void UpdateUI(float value)
    {
        Amount.Text = value.ToString(CultureInfo.InvariantCulture);
        HealthBar.Value = value / GlobalManager.playerState.MaxHealth.Value * 100;
    }

    public override void _ExitTree()
    {
        GlobalManager.playerState.Health.OnValueChanged -= UpdateUI;
    }
}