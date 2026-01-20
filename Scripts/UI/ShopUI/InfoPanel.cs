using AutoBattlerRoguelike.Scripts.UI.ShopUI;
using Godot;

public partial class InfoPanel : Control
{
    private AbilityInfoUI abilityInfoUI;
    private TraitInfoUI traitInfoUI;

    public override void _Ready()
    {
        abilityInfoUI = GetNode<AbilityInfoUI>("AbilityInfoUI");
        traitInfoUI = GetNode<TraitInfoUI>("TraitInfo");
    }

    public void ShowAbility(AbilityResource ability)
    {
        traitInfoUI.Clear();
        abilityInfoUI.Show();
        abilityInfoUI.ShowAbility(ability);
    }

    public void ShowTrait(AbilityTrait trait, int count)
    {
        abilityInfoUI.Hide();
        traitInfoUI.ShowTrait(trait, count);
    }

    public void Clear()
    {
        abilityInfoUI.Hide();
        traitInfoUI.Clear();
    }
}