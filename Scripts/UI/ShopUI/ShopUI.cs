using Godot;

public partial class ShopUI : Node
{
    [Export] private ShopItem shopItemUI1;
    [Export] private ShopItem shopItemUI2;
    [Export] private ShopItem shopItemUI3;
    [Export] private InfoPanel infoPanel;

    public override void _Ready()
    {
        SetupHover(shopItemUI1);
        SetupHover(shopItemUI2);
        SetupHover(shopItemUI3);

        PrepareShopItems();
        GetNode<ShopReroll>("Reroll").OnRerollShop += PrepareShopItems;
    }

    private void SetupHover(ShopItem item)
    {
        if (item == null) return;
        item.MouseEntered += () =>
        {
            if (item.abilityResource != null && infoPanel != null)
            {
                infoPanel.ShowAbility(item.abilityResource);
            }
        };
        item.MouseExited += () => infoPanel?.Clear();
    }

    private void PrepareShopItems()
    {
        PrepareShopItem(shopItemUI1);
        PrepareShopItem(shopItemUI2);
        PrepareShopItem(shopItemUI3);
    }

    private void PrepareShopItem(ShopItem shopItemUI)
    {
        var ability = GlobalManager.RollAbility();
        shopItemUI.GetNode<Label>("Title").Text = ability.Name;
        shopItemUI.GetNode<Label>("Price").Text = ability.Price.ToString();
        shopItemUI.GetNode<TextureRect>("Icon").Texture = ability.Icon;
        shopItemUI.abilityResource = ability;
        shopItemUI.Show();
    }
}