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

    private void DisplayAbilities(Array<PlayerAbilityResource> abilities)
    {
        foreach (var ability in GetChildren())
        {
            ability.QueueFree();
        }
        
        var radius = 160f;

        var abilitiesCount = abilities.Count;
        for (int i = 0; i < abilities.Count; i++)
        {
            var abilityResource = abilities[i];
            var ability = abilityInLoopScene.Instantiate<Control>();

            float angle = -90 + i * 360f / abilitiesCount;

            float angleRad = Mathf.DegToRad(angle);
            var dir = Vector2.FromAngle(angleRad);
            var pos = dir * radius;
            ability.Position = pos;
            AddChild(ability);

            ability.GetNode<Label>("Label").Text = abilityResource.AbilityResource.name;
            ability.GetNode<Label>("Level").Text = abilityResource.Level.ToString();
            ability.GetNode<Label>("Copies").Text = new string('-', abilityResource.Copies);
            ability.GetNode<TextureRect>("Icon").Texture = abilityResource.AbilityResource.icon;
            
        }
    }
}
