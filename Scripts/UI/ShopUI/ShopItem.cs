using Godot;

public partial class ShopItem : Control
{
    public AbilityResource abilityResource;

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
        {
            if (GlobalManager.playerState.Gold.Value >= abilityResource.Price)
            {
                var added = GlobalManager.playerState.TryAddAbility(abilityResource);
                if (added)
                {
                    GlobalManager.playerState.Gold.Subtract(abilityResource.Price);
                    Hide();
                }
                else
                {
                    // Feedback is already printed in TryAddAbility when capacity is full
                }
            }
            else
            {
                GD.Print("Not enough gold to buy this ability.");
            }
        }
    }
}