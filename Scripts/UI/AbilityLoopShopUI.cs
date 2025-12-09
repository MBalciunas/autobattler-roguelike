using Godot;

public partial class AbilityLoopShopUI : Control
{
    [Export] private PackedScene abilityInLoopScene;

    
    public override void _Ready()
    {
        var radius = 160f;

        var abilitiesCount = GlobalManager.playerState.AbilitiesInLoop.Count;
        for (int i = 0; i < GlobalManager.playerState.AbilitiesInLoop.Count; i++)
        {
            AbilityResource abilityResource = GlobalManager.playerState.AbilitiesInLoop[i];
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
