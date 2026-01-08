using Godot;
using Godot.Collections;

public partial class AbilityInLoopDraggable : Control
{
    [Export] public int Index;

    // Data for drag preview
    public Texture2D IconTexture;
    public string AbilityName = string.Empty;

    private TextureRect iconNode;

    public override void _Ready()
    {
        iconNode = GetNode<TextureRect>("Icon");
        IconTexture = iconNode.Texture;
    }

    public override Variant _GetDragData(Vector2 at)
    {
        var data = new Dictionary
        {
            { "type", "ability_loop_index" },
            { "index", Index }
        };

        // Use the same icon texture as a preview and hide this control while dragging
        if (IconTexture != null)
        {
            var previewIcon = new TextureRect
            {
                Texture = IconTexture,
                ExpandMode = iconNode.ExpandMode,
                Modulate = new Color(1, 1, 1, 0.85f), // slight transparency while dragging
                CustomMinimumSize = iconNode.Size
            };
            // Try to mirror visual size/scale
            previewIcon.Scale = iconNode.Scale;
            SetDragPreview(previewIcon);
        }
        else
        {
            // Fallback minimal preview
            var fallback = new ColorRect { Color = new Color(1, 1, 1, 0.5f), CustomMinimumSize = new Vector2(48, 48) };
            SetDragPreview(fallback);
        }

        // Hide original so it looks like it's being picked up
        Hide();

        return data;
    }

    public override bool _CanDropData(Vector2 at, Variant data)
    {
        if (data.VariantType != Variant.Type.Dictionary) return false;
        var dict = (Dictionary)data;
        if (!dict.ContainsKey("type") || (string)dict["type"] != "ability_loop_index") return false;
        if (!dict.ContainsKey("index")) return false;
        int fromIndex = (int)dict["index"];
        return fromIndex != Index; // can't drop onto itself
    }

    public override void _DropData(Vector2 at, Variant data)
    {
        var dict = (Dictionary)data;
        int fromIndex = (int)dict["index"];
        // Move the dragged ability to this item's index position
        var moved = GlobalManager.playerState.MoveAbility(fromIndex, Index);
        if (!moved)
        {
            GD.Print($"Reorder failed: {fromIndex} -> {Index}");
        }
    }

    public override void _Notification(int what)
    {
        // Ensure the item reappears after drag ends (including when dropped outside targets)
        if (what == NotificationDragEnd)
        {
            Show();
        }
    }
}
