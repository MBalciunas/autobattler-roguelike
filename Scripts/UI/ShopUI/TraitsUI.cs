using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;

public partial class TraitsUI : Control
{
    [Export] private PackedScene traitUIScene;
    [Export] private InfoPanel infoPanel;
    private VBoxContainer traitsContainer;
    private System.Collections.Generic.Dictionary<AbilityTrait, int> _traitCounts = new();

    public override void _Ready()
    {
        traitsContainer = GetNode<VBoxContainer>("TraitsContainer");
        UpdateTraitsUI(GlobalManager.playerState.AbilitiesInLoop);
        GlobalManager.playerState.OnAbilitiesChanged += UpdateTraitsUI;
    }

    private void UpdateTraitsUI(Array<PlayerAbilityResource> playerAbilities)
    {
        foreach (var trait in traitsContainer.GetChildren())
        {
            trait.QueueFree();
        }

        var abilities = playerAbilities.Select(a => a.AbilityResource).ToList();

        _traitCounts = abilities.SelectMany(a => a.Traits)
            .GroupBy(t => t)
            .ToDictionary(g => g.Key, g => g.Count());

        var sortedTraits = _traitCounts.OrderByDescending(trait => trait.Value);

        foreach (var keyValuePair in sortedTraits)
        {
            var traitNode = traitUIScene.Instantiate<Control>();
            traitNode.GetNode<Label>("TraitName").Text = keyValuePair.Key + " - " + keyValuePair.Value;
            traitsContainer.AddChild(traitNode);

            SetupTraitHover(traitNode, keyValuePair.Key, keyValuePair.Value);
        }
    }

    private void SetupTraitHover(Control traitNode, AbilityTrait trait, int count)
    {
        traitNode.MouseEntered += () => OnTraitMouseEntered(trait, count);
        traitNode.MouseExited += OnTraitMouseExited;
    }

    private void OnTraitMouseEntered(AbilityTrait trait, int count)
    {
        infoPanel?.ShowTrait(trait, count);
    }

    private void OnTraitMouseExited()
    {
        infoPanel?.Clear();
    }

    public override void _ExitTree()
    {
        GlobalManager.playerState.OnAbilitiesChanged -= UpdateTraitsUI;
    }
}