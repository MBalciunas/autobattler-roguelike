using System.Linq;
using Godot;
using Godot.Collections;

public partial class TraitsUI : Control
{
    [Export] private PackedScene traitUIScene;
    private VBoxContainer traitsContainer;
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

        var traits = abilities.SelectMany(a => a.Traits)
            .GroupBy(t => t)
            .ToDictionary(g => g.Key, g => g.Count())
            .OrderByDescending(trait => trait.Value);

        foreach (var keyValuePair in traits)
        {
            var trait = traitUIScene.Instantiate<Control>();
            trait.GetNode<Label>("TraitName").Text = keyValuePair.Key + " - " + keyValuePair.Value;
            // trait.GetNode<Label>("TraitCount").Text = keyValuePair.Value.ToString();
            traitsContainer.AddChild(trait);
        }
    }

    public override void _ExitTree()
    {
        GlobalManager.playerState.OnAbilitiesChanged -= UpdateTraitsUI;
    }
}