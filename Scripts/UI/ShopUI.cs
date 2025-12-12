using System.Linq;
using Godot;

public partial class ShopUI : Node
{
    [Export] private ShopItem shopItemUI1;
    [Export] private ShopItem shopItemUI2;
    [Export] private ShopItem shopItemUI3;
    

    public override void _Ready()
    {
        PrepareShopItems();
        GetNode<ShopReroll>("Reroll").OnRerollShop +=  PrepareShopItems;
    }

    private void PrepareShopItems()
    {
        PrepareShopItem(shopItemUI1);
        PrepareShopItem(shopItemUI2);
        PrepareShopItem(shopItemUI3);
    }

    private void PrepareShopItem(ShopItem shopItemUI)
    {
        var abilityName = GlobalManager.Abilities.Keys.ToArray()[GD.Randi() % GlobalManager.Abilities.Count];

        var ability = GlobalManager.Abilities[abilityName];
        shopItemUI.GetNode<Label>("Title").Text = ability.name;
        shopItemUI.GetNode<Label>("Price").Text = ability.price.ToString();
        shopItemUI.GetNode<TextureRect>("Icon").Texture = ability.icon;
        shopItemUI.abilityResource = ability;
        shopItemUI.Show();
    }
}