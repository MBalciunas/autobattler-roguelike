using Godot;

public partial class LoopUI : Control
{
    [Export] private PackedScene abilityInLoopScene;

    private AbilityExecutor abilityExecutor;

    private bool isRotationFinished;
    private double rotationAngle;
    private int rotateTimes = 0;

    public override void _Ready()
    {
        abilityExecutor = GlobalManager.Player.GetNode<AbilityExecutor>("AbilityExecutor");
        var radius = 100f;

        var abilitiesCount = GlobalManager.playerState.AbilitiesInLoop.Count;
        rotationAngle = 360.0 / abilitiesCount;
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

    public override void _Process(double delta)
    {
        if (abilityExecutor.cooldownTimer.TimeLeft == 0)
        {
            if (!isRotationFinished)
            {
                isRotationFinished = true;
                rotateTimes += 1;
            }
        }
        else
        {
            isRotationFinished = false;
            GD.Print(rotateTimes);
            // GD.Print((float)(abilityExecutor.cooldownTimer.TimeLeft / abilityExecutor.cooldownTimer.WaitTime *
            //                                 rotationAngle) * rotateTimes);
            Rotation = (float)(rotateTimes * Mathf.DegToRad(rotationAngle) + Mathf.DegToRad((1 - abilityExecutor.cooldownTimer.TimeLeft / abilityExecutor.cooldownTimer.WaitTime) * rotationAngle));
        }
    }
}