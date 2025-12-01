using Godot;

public partial class EnemySpawner : Node
{
    [Export] private PackedScene enemyScene;
    private Timer spawnTimer;

    public override void _Ready()
    {
        spawnTimer = GetNode<Timer>("SpawnTimer");
        spawnTimer.Timeout += SpawnEnemies;
    }

    private void SpawnEnemies()
    {
        var enemy = enemyScene.Instantiate() as Enemy;
        AddChild(enemy);
        var playerPos = GetNode<Node2D>("../Player").GlobalPosition;
        var   minDistance = 200f;

        enemy.GlobalPosition = GetRandomOnScreenPosAwayFromPlayer(playerPos, minDistance);
    }
    
    private Vector2 GetRandomOnScreenPosAwayFromPlayer(Vector2 playerPos, float minDistance)
    {
        var size = GetViewport().GetVisibleRect().Size;

        Vector2 candidate = playerPos;

        for (int i = 0; i < 8; i++)
        {
            float x = (float)GD.RandRange(0, size.X);
            float y = (float)GD.RandRange(0, size.Y);
            candidate = new Vector2(x, y);
            if (candidate.DistanceTo(playerPos) >= minDistance)
                return candidate;
        }

        // Fallback: push the last candidate out to minDistance, then clamp to screen
        var dir = (candidate - playerPos);
        if (dir.LengthSquared() < 0.0001f) dir = Vector2.Right; // avoid NaN if same point
        var outPos = playerPos + dir.Normalized() * minDistance;
        outPos.X = Mathf.Clamp(outPos.X, 0, size.X);
        outPos.Y = Mathf.Clamp(outPos.Y, 0, size.Y);
        return outPos;
    }
}
