using Godot;

public partial class DamageTakenUI : Node2D
{
    private Label label;
    public override void _Ready()
    {
        label = GetNode<Label>("Label");
    }

    public void Init(float damage, ElementType damageType)
    {
        label.Text = $"{damage:0.#}";

        Color color = Colors.White;

        switch (damageType)
        {
            case ElementType.Poison:
                color = Colors.Green;
                break;

            case ElementType.Bleed:
                color = Colors.Red;
                break;
        }

        label.AddThemeColorOverride("font_color", color);
    }
}
