using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.UI.ShopUI;

public partial class AbilityInfoUI : Control
{
    private Label _title;
    private Label _rarity;
    private Label _price;
    private Label _traits;
    private Label _description;
    private Label level1Effect;
    private Label level2Effect;
    private Label level3Effect;
    private ColorRect _background;

    public override void _Ready()
    {
        _title = GetNode<Label>("Title");
        _rarity = GetNode<Label>("Rarity");
        _price = GetNode<Label>("Price");
        _traits = GetNode<Label>("Traits");
        _description = GetNode<Label>("Description");
        level1Effect = GetNode<Label>("Level1Effect");
        level2Effect = GetNode<Label>("Level2Effect");
        level3Effect = GetNode<Label>("Level3Effect");
        _background = GetNode<ColorRect>("Background");
        Clear();
    }

    public void ShowAbility(AbilityResource ability)
    {
        if (ability == null)
        {
            Clear();
            return;
        }

        _title.Text = ability.Name;
        _rarity.Text = $"Rarity: {ability.Rarity}";
        _price.Text = $"Price: {ability.Price}$";
        _description.Text = ability.Description;
        level1Effect.Text = "*   | " + ability.Level1Effect;
        level2Effect.Text = "**  | " + ability.Level2Effect;
        level3Effect.Text = "*** | " + ability.Level3Effect;

        if (ability.Traits != null && ability.Traits.Count > 0)
        {
            var traits = string.Join(", ", ability.Traits.Select(t => t.ToString()));
            _traits.Text = $"Traits: {traits}";
        }
        else
        {
            _traits.Text = "Traits: -";
        }

        _background.Modulate = GetRarityColor(ability.Rarity).Darkened(0.6f);
        Show();
    }

    private void Clear()
    {
        _title.Text = "Hover an item to see details";
        _rarity.Text = string.Empty;
        _price.Text = string.Empty;
        _traits.Text = string.Empty;
        _description.Text = string.Empty;
        Show();
    }

    private Color GetRarityColor(AbilityRarity rarity)
    {
        return rarity switch
        {
            AbilityRarity.Common => new Color(0.6f, 0.6f, 0.6f),
            AbilityRarity.Uncommon => new Color(0.3f, 0.8f, 0.3f),
            AbilityRarity.Rare => new Color(0.3f, 0.5f, 1.0f),
            AbilityRarity.Epic => new Color(0.7f, 0.3f, 0.9f),
            AbilityRarity.Legendary => new Color(1.0f, 0.8f, 0.2f),
            _ => new Color(1, 1, 1)
        };
    }
}