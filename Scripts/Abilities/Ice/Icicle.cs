using Godot;

public partial class Icicle : Ability
{
    [Export] private PackedScene fireboltProjectileScene;
    public override void Execute()
    {
        GD.Print("Start Icicle");
        ExecuteIcicle();
    }

    private async void ExecuteIcicle()
    {
        await ToSignal(GetTree().CreateTimer(2f), SceneTreeTimer.SignalName.Timeout);
        GD.Print("Finish Icicle");
        EmitSignal(Ability.SignalName.Finished, this);
    }
}
