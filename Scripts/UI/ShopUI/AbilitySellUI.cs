using Godot;
using Godot.Collections;

namespace AutoBattlerRoguelike.Scripts.UI.ShopUI;

public partial class AbilitySellUI : Control
{
    private Label _label;

    public override void _Ready()
    {
        _label = GetNode<Label>("SellValue");
        _label.Text = "Sell Ability";
    }

    public override bool _CanDropData(Vector2 at, Variant data)
    {
        if (data.VariantType != Variant.Type.Dictionary) return false;
        var dict = (Dictionary)data;

        if (!dict.ContainsKey("type") || !dict.ContainsKey("index")) return false;
        if ((string)dict["type"] != "ability_loop_index") return false;

        int fromIndex = (int)dict["index"];

        // Update hover text while dragging over the sell zone
        int sellValue = GlobalManager.playerState.GetSellValueForAbility(fromIndex);
        _label.Text = $"Sell for {sellValue}";
        _label.Visible = true;

        return true;
    }

    public override void _DropData(Vector2 at, Variant data)
    {
        _label.Text = "Sell Ability";

        var dict = (Dictionary)data;
        int fromIndex = (int)dict["index"];

        GlobalManager.playerState.SellAbility(fromIndex);
    }

    public override void _Notification(int what)
    {
        // Hide label when drag ends anywhere (drop success or cancel)
        if (what == NotificationDragEnd)
            _label.Text = "Sell Ability";
    }

    public override void _ExitTree()
    {
        if (_label != null)
            _label.Text = "Sell Ability";
    }
}