using Godot;

public partial class ShopReroll : Control
{
    [Signal] public delegate void OnRerollShopEventHandler();

    [Export] private int rerollPrice;

    public override void _Ready()
    {
        GetNode<Label>("PriceLabel").Text = rerollPrice + "$";
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
        {
            if (GlobalManager.playerState.Gold.Value >= rerollPrice)
            {
                GlobalManager.playerState.Gold.Subtract(rerollPrice);
                EmitSignal(SignalName.OnRerollShop);
            }
        }
    }
}
