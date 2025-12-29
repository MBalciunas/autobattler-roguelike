using Godot;

public partial class LoopUI : Control
{
    [Export] private PackedScene abilityInLoopScene;

    private AbilityExecutor abilityExecutor;

    private double rotationAngle;

    public override void _Ready()
    {
        abilityExecutor = GlobalManager.Player.GetNode<AbilityExecutor>("AbilityExecutor");
        var radius = 100f;

        var abilitiesCount = GlobalManager.playerState.AbilitiesInLoop.Count;
        rotationAngle = 360.0 / abilitiesCount;
        for (int i = 0; i < GlobalManager.playerState.AbilitiesInLoop.Count; i++)
        {
            var abilityResource = GlobalManager.playerState.AbilitiesInLoop[i];
            var ability = abilityInLoopScene.Instantiate<Control>();

            float angle = -90 + i * 360f / abilitiesCount;

            float angleRad = Mathf.DegToRad(angle);
            var dir = Vector2.FromAngle(angleRad);
            var pos = dir * radius;
            ability.Position = pos;
            AddChild(ability);

            ability.GetNode<Label>("Label").Text = abilityResource.AbilityResource.Name;
            ability.GetNode<TextureRect>("Icon").Texture = abilityResource.AbilityResource.Icon;
        }
    }

    public override void _Process(double delta)
    {
        if (abilityExecutor.cooldownTimer.TimeLeft == 0) return;
        
        float progress = 1 - (float)(abilityExecutor.cooldownTimer.TimeLeft / abilityExecutor.cooldownTimer.WaitTime);
        Rotation = -(float)(abilityExecutor.nextAbilityIndex * Mathf.DegToRad(rotationAngle) + Mathf.DegToRad(progress * rotationAngle));
    }
}