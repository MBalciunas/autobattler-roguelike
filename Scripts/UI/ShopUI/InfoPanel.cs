using AutoBattlerRoguelike.Scripts.UI.ShopUI;
using Godot;

public partial class InfoPanel : Control
{
    private AbilityInfoUI abilityInfoUI;

    public override void _Ready()
    {
        abilityInfoUI = GetNode<AbilityInfoUI>("AbilityInfoUI");
    }

    public void ShowAbility(AbilityResource ability)
    {
        abilityInfoUI.Show();
        abilityInfoUI.ShowAbility(ability);
    }

    public void Clear()
    {
        abilityInfoUI.Hide();
    }
}