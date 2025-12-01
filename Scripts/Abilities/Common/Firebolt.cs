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
        await ToSignal(GetTree().CreateTimer(2f), SceneTreeTimer.SignalName.Timeout);
        EmitSignal(Ability.SignalName.Finished, this);
    }
}
