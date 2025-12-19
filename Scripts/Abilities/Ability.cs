using Godot;

public abstract partial class Ability : Node2D
{
    public int Level = 1;

    public async void Execute()
    {
        ExecuteAbility();
        await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);
        EmitSignal(SignalName.Finished, this);
    }

    protected abstract void ExecuteAbility();
    [Signal] public delegate void FinishedEventHandler(Ability ability);
}
