using Godot;

public partial class Firebolt : Ability
{
    [Export] private PackedScene fireboltProjectileScene;
    public override void Execute()
    {
        GD.Print("Start Firebolt");
        ExecuteFirebolt();
    }

    private async void ExecuteFirebolt()
    {
        await ToSignal(GetTree().CreateTimer(2f), SceneTreeTimer.SignalName.Timeout);
        GD.Print("Finish Firebolt");
        EmitSignal(Ability.SignalName.Finished, this);
    }
}
