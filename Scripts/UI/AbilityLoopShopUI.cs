using Godot;
using Godot.Collections;

public partial class AbilityLoopShopUI : Control
{
    [Export] private PackedScene abilityInLoopScene;

    
    public override void _Ready()
    {
        GlobalManager.playerState.OnAbilitiesChanged += DisplayAbilities;
        DisplayAbilities(GlobalManager.playerState.AbilitiesInLoop);
    }

    public override void _ExitTree()
    {
        GlobalManager.playerState.OnAbilitiesChanged -= DisplayAbilities;
    }

    private void DisplayAbilities(Array<AbilityResource> abilities)
    {
        foreach (var ability in GetChildren())
        {
            ability.QueueFree();
        }
        
        var radius = 160f;

        var abilitiesCount = abilities.Count;
        for (int i = 0; i < abilities.Count; i++)
        {
            AbilityResource abilityResource = abilities[i];
            var ability = abilityInLoopScene.Instantiate<Control>();

            float angle = -90 + i * 360f / abilitiesCount;

            float angleRad = Mathf.DegToRad(angle);
            var dir = Vector2.FromAngle(angleRad);
            var pos = dir * radius;
            ability.Position = pos;
            AddChild(ability);

            ability.GetNode<Label>("Label").Text = abilityResource.name;
            ability.GetNode<TextureRect>("Icon").Texture = abilityResource.icon;
        }
    }
}
