using System.Globalization;
using Godot;

public partial class DamageTakenUI : Node2D
{
    private Label label;
    public override void _Ready()
    {
        label = GetNode<Label>("Label");
    }

    public void Init(float damage)
    {
        label.Text = $"{damage:0.#}";
    }
}
