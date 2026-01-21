using AutoBattlerRoguelike.Scripts;
using Godot;

namespace AutoBattlerRoguelike.Scripts.UI.ShopUI;

public partial class TraitInfoUI : Control
{
    private Label _title;
    private Label _description;
    private ColorRect _background;
    private VBoxContainer _effectsContainer;

    private static readonly Color ActiveTierColor = new Color(0.3f, 0.9f, 0.3f);
    private static readonly Color InactiveTierColor = new Color(0.5f, 0.5f, 0.5f);

    public override void _Ready()
    {
        _title = GetNode<Label>("Title");
        _description = GetNode<Label>("Description");
        _background = GetNode<ColorRect>("Background");
        _effectsContainer = GetNode<VBoxContainer>("Effects");
        Clear();
    }

    public void ShowTrait(AbilityTrait trait, int currentCount)
    {
        var traitData = GlobalManager.Traits[trait];
        if (traitData == null)
        {
            Clear();
            return;
        }

        _title.Text = traitData.Name;
        _description.Text = traitData.Description;

        var tierLabels = _effectsContainer.GetChildren();
        for (int i = 0; i < tierLabels.Count; i++)
        {
            var tierLabel = tierLabels[i] as Label;
            if (tierLabel == null) continue;

            if (i < traitData.Tiers.Count)
            {
                var tier = traitData.Tiers[i];
                tierLabel.Text = $"({tier.Required}) {tier.Effect}";
                tierLabel.Visible = true;

                bool isActive = currentCount >= tier.Required;
                tierLabel.Modulate = isActive ? ActiveTierColor : InactiveTierColor;
            }
            else
            {
                tierLabel.Visible = false;
            }
        }

        Show();
    }

    public void Clear()
    {
        _title.Text = string.Empty;
        _description.Text = string.Empty;

        foreach (var child in _effectsContainer.GetChildren())
        {
            if (child is Label label)
            {
                label.Text = string.Empty;
                label.Visible = false;
            }
        }

        Hide();
    }
}
