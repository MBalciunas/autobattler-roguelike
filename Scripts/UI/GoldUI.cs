using Godot;

public partial class GoldUI : Label
{
    public override void _Ready()
    {
        Text = "Gold: " + GlobalManager.playerState.Gold.Value;
        GlobalManager.playerState.Gold.OnValueChanged += value => Text = "Gold: " + value;
    }
}
