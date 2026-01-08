using Godot;

public partial class BuyXPButton : Button
{
    [Export] public int GoldToSpendPerClick = 1;
    [Export] public int RateXpPerGold = 2; // gold needed per 1 xp (1 gold -> 2 XP)

    public override void _Ready()
    {
        Text = $"Buy XP ({GoldToSpendPerClick}g -> {Mathf.FloorToInt(GoldToSpendPerClick * RateXpPerGold)}xp)";
        GlobalManager.playerState.Gold.OnValueChanged += OnGoldChanged;
        OnGoldChanged(GlobalManager.playerState.Gold.Value);
        Pressed += OnPressed;
    }

    private void OnPressed()
    {
        GlobalManager.playerState.SpendGoldForXp(GoldToSpendPerClick, RateXpPerGold);
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