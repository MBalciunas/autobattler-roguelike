using System.Linq;
using Godot;

public partial class Firebolt : Ability
{
    [Export] private PackedScene fireboltProjectileScene;
    [Export] private float damage;

    public override void Execute()
    {
        ExecuteFirebolt();
    }

    private async void ExecuteFirebolt()
    {
        await ToSignal(GetTree().CreateTimer(0.1f), SceneTreeTimer.SignalName.Timeout);

        var firebolt = fireboltProjectileScene.Instantiate<Projectile>();
        firebolt.GlobalPosition = GlobalPosition;
        var enemy = GetTree().GetNodesInGroup("Enemies")
            .Cast<Node2D>()
            .OrderBy(e => e.GlobalPosition.DistanceTo(GlobalPosition))
            .ToList().FirstOrDefault();

        if (enemy != null)
        {
            var direction = (enemy.GlobalPosition - GlobalPosition).Normalized();
            firebolt.Rotation = direction.Angle();
            firebolt.damage = damage;
            GetTree().Root.GetNode("MainLevel").AddChild(firebolt);
        }

        EmitSignal(Ability.SignalName.Finished, this);
    }
}