using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Common;

public partial class FieryCharge : Ability
{
    [Export] private PackedScene fireGroundScene;
    private float chargeDistance = 600f;
    private float fireSpawnInterval = 50f;

    private bool isCharging;
    private Vector2 lastFirePos;
    private float damage;
    private float duration;

    protected override void ExecuteAbility()
    {
        var enemy = GlobalManager.GetEnemiesSortedByClosest().FirstOrDefault();
        if (enemy == null) return;

        var stats = GetStats();
        damage = stats.damage;
        duration = stats.duration;

        var startPos = GlobalManager.Player.GlobalPosition;
        var direction = (enemy.GlobalPosition - startPos).Normalized();
        var targetPos = startPos + direction * chargeDistance;

        // Clamp to bounds
        targetPos = ClampToBounds(targetPos);

        // Start charging and spawning fire
        isCharging = true;
        lastFirePos = startPos;
        SpawnFireAt(startPos);
        GlobalManager.Player.isInvulnerable = true;

        // Dash to target
        var tween = GetTree().CreateTween();
        tween.TweenProperty(GlobalManager.Player, "global_position", targetPos, 0.4f);
        tween.TweenCallback(Callable.From(() => {
            isCharging = false;
            GlobalManager.Player.isInvulnerable = false;
        }));
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (isCharging)
        {
            var currentPos = GlobalManager.Player.GlobalPosition;
            if (currentPos.DistanceTo(lastFirePos) >= fireSpawnInterval)
            {
                SpawnFireAt(currentPos);
                lastFirePos = currentPos;
            }
        }
    }

    private void SpawnFireAt(Vector2 position)
    {
        var fireGround = fireGroundScene.Instantiate<FireGroundEffect>();
        fireGround.Init((damage, duration));
        fireGround.GlobalPosition = position;
        GetTree().Root.GetNode("MainLevel").AddChild(fireGround);
    }

    private Vector2 ClampToBounds(Vector2 position)
    {
        var screenRect = GetViewport().GetVisibleRect();
        float bottomUiHeight = 150f;
        float buffer = 15f;

        var minX = screenRect.Position.X + buffer;
        var maxX = screenRect.End.X - buffer;
        var minY = screenRect.Position.Y + buffer;
        var maxY = screenRect.End.Y - buffer - bottomUiHeight;

        return new Vector2(
            Mathf.Clamp(position.X, minX, maxX),
            Mathf.Clamp(position.Y, minY, maxY)
        );
    }
}
