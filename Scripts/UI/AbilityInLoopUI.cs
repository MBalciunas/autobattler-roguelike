using Godot;

public partial class AbilityInLoopUI : Control
{
    public override void _Process(double delta)
    {
        Rotation = -GetParent<Control>().Rotation; 
    }
}
