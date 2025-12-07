using System.Linq;
using Godot;

public partial class Icicle : Ability
{
    [Export] private PackedScene icicleProjectileScene;
    [Export] private float damage;

    public override void Execute()
    {
        ExecuteIcicle();
    }

    private async void ExecuteIcicle()
    {
        await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);

        var icicle = icicleProjectileScene.Instantiate<Projectile>();
        icicle.GlobalPosition = GlobalPosition;
        var enemy = GetTree().GetNodesInGroup("Enemies")
            .Cast<Node2D>()
            .OrderBy(e => e.GlobalPosition.DistanceTo(GlobalPosition))
            .ToList().FirstOrDefault();

        if (enemy != null)
        {
            var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();
            icicle.Rotation = direction.Angle();
            icicle.damage = damage;
            GetTree().Root.GetNode("MainLevel").AddChild(icicle);
        }

        EmitSignal(Ability.SignalName.Finished, this);
    }
}