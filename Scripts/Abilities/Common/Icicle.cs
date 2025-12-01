using Godot;

public partial class Icicle : Ability
{
    [Export] private PackedScene fireboltProjectileScene;
    public override void Execute()
    {
        ExecuteIcicle();
    }

    private async void ExecuteIcicle()
    {
        await ToSignal(GetTree().CreateTimer(2f), SceneTreeTimer.SignalName.Timeout);
        EmitSignal(Ability.SignalName.Finished, this);
    }
}
