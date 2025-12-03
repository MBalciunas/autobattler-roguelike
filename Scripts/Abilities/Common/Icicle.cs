using Godot;

public partial class Icicle : Ability
{
    [Export] private PackedScene icicleProjectileScene;
    public override void Execute()
    {
        ExecuteIcicle();
    }

    private async void ExecuteIcicle()
    {
        GD.Print("ExecuteIcicle");
        await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
        GD.Print("ExecuteIcicle finished");
        EmitSignal(Ability.SignalName.Finished, this);
    }
}
