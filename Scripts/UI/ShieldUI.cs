using System.Globalization;
using Godot;

public partial class ShieldUI : Control
{
    private Label Amount;
    private Label Timer;
    private TextureProgressBar ShieldBar;

    public override void _Ready()
    {
        Amount = GetNode<Label>("Amount");
        Timer = GetNode<Label>("Timer");
        ShieldBar = GetNode<TextureProgressBar>("Bar");
        UpdateUI(GlobalManager.playerState.Shield.Value);
        UpdateDurationUI(GlobalManager.playerState.ShieldDuration.Value);
        ShieldBar.Visible = false;
        Amount.Visible = false;
        GlobalManager.playerState.Shield.OnValueChanged += UpdateUI;
        GlobalManager.playerState.ShieldDuration.OnValueChanged += UpdateDurationUI;
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
            Amount.Text = $"{value:0.##}";
            // ShieldBar.Value = value / GlobalManager.playerState.MaxHealth.Value * 100;
        }
    }
    
    private void UpdateDurationUI(int value)
    {
        if (value <= 0)
        {
            Timer.Hide();
        }
        else
        {
            Timer.Show();
            Timer.Text = value.ToString(CultureInfo.InvariantCulture);
        }
    }

    public override void _ExitTree()
    {
        GlobalManager.playerState.Shield.OnValueChanged -= UpdateUI;
        GlobalManager.playerState.ShieldDuration.OnValueChanged -= UpdateDurationUI;

    }
}