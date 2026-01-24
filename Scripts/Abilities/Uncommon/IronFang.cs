using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Uncommon;

public partial class IronFang : Ability
{
    [Export] private PackedScene ironFangWallScene;

    protected override void ExecuteAbility()
    {
        var enemy = GlobalManager.GetEnemiesSortedByClosest().FirstOrDefault();
        if (enemy == null) return;

        var stats = GetStats();
        float damage = stats.damage;
        float width = stats.width > 0 ? stats.width : 100;

        // Direction from player to enemy
        var directionToEnemy = (enemy.GlobalPosition - GlobalPosition).Normalized();
        // Perpendicular direction (walls come from the sides)
        var perpendicular = new Vector2(-directionToEnemy.Y, directionToEnemy.X);

        // Target position is the enemy's position
        var targetPosition = enemy.GlobalPosition;

        // Spawn two walls on opposite sides
        SpawnWall(targetPosition + perpendicular * width, targetPosition, damage, width);
        SpawnWall(targetPosition - perpendicular * width, targetPosition, damage, width);
    }

    private void SpawnWall(Vector2 startPosition, Vector2 targetPosition, float damage, float width)
    {
        var wall = ironFangWallScene.Instantiate<IronFangWall>();
        wall.Init(damage, targetPosition, width);
        wall.GlobalPosition = startPosition;

        // Rotate the wall to face the target
        var direction = (targetPosition - startPosition).Normalized();
        wall.Rotation = direction.Angle();

        GetTree().Root.GetNode("MainLevel").AddChild(wall);
    }
}
