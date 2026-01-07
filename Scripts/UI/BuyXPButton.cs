using Godot;

public partial class BuyXPButton : Button
{
    [Export] public int GoldToSpendPerClick = 1;
    [Export] public float RateGoldPerXP = 0.5f; // gold needed per 1 xp (1 gold -> 2 XP)

    public override void _Ready()
    {
        Text = $"Buy XP ({GoldToSpendPerClick}g -> {Mathf.FloorToInt(GoldToSpendPerClick / RateGoldPerXP)}xp)";
        GlobalManager.playerState.Gold.OnValueChanged += OnGoldChanged;
        OnGoldChanged(GlobalManager.playerState.Gold.Value);
        Pressed += OnPressed;
    }

    private void OnPressed()
    {
        GlobalManager.playerState.SpendGoldForXP(GoldToSpendPerClick, RateGoldPerXP);
    }

    private void OnGoldChanged(int gold)
    {
        Disabled = gold < GoldToSpendPerClick;
    }

    public override void _ExitTree()
    {
        GlobalManager.playerState.Gold.OnValueChanged -= OnGoldChanged;
        Pressed -= OnPressed;
    }
}
