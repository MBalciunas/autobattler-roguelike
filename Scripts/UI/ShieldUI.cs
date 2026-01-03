using System.Globalization;
using Godot;

public partial class ShieldUI : Control
{
    private Label Amount;
    private TextureProgressBar ShieldBar;

    public override void _Ready()
    {
        Amount = GetNode<Label>("Amount");
        ShieldBar = GetNode<TextureProgressBar>("Bar");
        Amount.Text = GlobalManager.playerState.Shield.Value.ToString(CultureInfo.InvariantCulture);
        ShieldBar.Visible = false;
        Amount.Visible = false;
        GlobalManager.playerState.Shield.OnValueChanged += UpdateUI;
    }

    private void UpdateUI(float value)
    {
        if (value <= 0)
        {
            ShieldBar.Visible = false;
            Amount.Visible = false;
        }
        else
        {
            ShieldBar.Visible = true;
            Amount.Visible = true;
            Amount.Text = value.ToString(CultureInfo.InvariantCulture);
            // ShieldBar.Value = value / GlobalManager.playerState.MaxHealth.Value * 100;
        }
    }

    public override void _ExitTree()
    {
        GlobalManager.playerState.Health.OnValueChanged -= UpdateUI;
    }
}