using Godot;

public partial class ShopItem : Control
{
    public AbilityResource abilityResource;

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventMouseButton { Pressed: true, ButtonIndex: MouseButton.Left })
        {
            if (GlobalManager.playerState.Gold.Value >= abilityResource.price)
            {
                GlobalManager.playerState.Gold.Subtract(abilityResource.price);
                GlobalManager.playerState.AddAbility(abilityResource);
                Hide();
            }
        }
    }
}