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
                GlobalManager.playerState.Gold.Subtract(abilityResource.Price);
                GlobalManager.playerState.AddAbility(abilityResource);
                Hide();
            }
        }
    }
}