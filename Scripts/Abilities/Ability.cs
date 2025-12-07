using Godot;

public abstract partial class Ability : Node2D
{
    public int Level = 1;

    public abstract void Execute();
    [Signal] public delegate void FinishedEventHandler(Ability ability);
}
