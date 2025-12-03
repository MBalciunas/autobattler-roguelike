using Godot;

public partial class Firebolt : Ability
{
    [Export] private PackedScene fireboltProjectileScene;
    public override void Execute()
    {
        ExecuteFirebolt();
    }

    private async void ExecuteFirebolt()
    {
        GD.Print("ExecuteFirebolt");
        await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
        GD.Print("ExecuteFirebolt finished");
        EmitSignal(Ability.SignalName.Finished, this);
    }
}
