using Godot;

public partial class EnemySpawner : Node
{
    [Export] private PackedScene enemyScene;
    
    public int currentWave = 0;
    public int enemiesAlive = 0;
    
    private float buffer = 50f;
    private float bottomUiHeight = 150f;
    private Vector2 screenSize;
    
    [Signal]
    public delegate void OnWaveNumberChangedEventHandler(int currentWave, int totalWaves);

    public override void _Ready()
    {
        screenSize = new Vector2(
            ProjectSettings.GetSetting("display/window/size/viewport_width").AsInt32(),
            ProjectSettings.GetSetting("display/window/size/viewport_height").AsInt32()
        );
        
        StartWave();
    }

    private int GetEnemiesPerWave()
    {
        return GlobalManager.Level + currentWave / 2;
        // return 1;
    }

    private int GetTotalWaves()
    {
        return 1 + GlobalManager.Level / 2;
        // return 1;
    }

    private void StartWave()
    {
        currentWave++;
        EmitSignal(SignalName.OnWaveNumberChanged, currentWave, GetTotalWaves());
        SpawnWave();
    }

    private void SpawnWave()
    {
        int totalEnemies = GetEnemiesPerWave();
        int clusters = Mathf.Max(1, totalEnemies / 3);
        int enemiesPerCluster = totalEnemies / clusters;
        int remainder = totalEnemies % clusters;
        
        var playerPos = GetNode<Node2D>("../Player").GlobalPosition;
        
        for (int c = 0; c < clusters; c++)
        {
            var clusterCenter = GetRandomSpawnPosition(playerPos, 300f);
            int count = enemiesPerCluster + (c < remainder ? 1 : 0);
            
            for (int i = 0; i < count; i++)
            {
                var enemy = enemyScene.Instantiate<Enemy>();
                
                var offset = new Vector2(
                    GD.RandRange(-150, 150),
                    GD.RandRange(-150, 150)
                );
                
                var finalPos = clusterCenter + offset;
                finalPos.X = Mathf.Clamp(finalPos.X, buffer, screenSize.X - buffer);
                finalPos.Y = Mathf.Clamp(finalPos.Y, buffer, screenSize.Y - buffer - bottomUiHeight);
                
                enemy.GlobalPosition = finalPos;
                enemy.Died += OnEnemyDied;
                enemiesAlive++;

                CallDeferred("add_child", enemy);
            }
        }
    }

    private void OnEnemyDied()
    {
        if (GetTree().GetNodesInGroup("Enemies").Count == 0)
        {
            if (currentWave < GetTotalWaves())
            {
                StartWave();
            }
            else
            {
                GameManager.Instance.FinishLevel();
            }
        }
    }

    private Vector2 GetRandomSpawnPosition(Vector2 playerPos, float minDistance)
    {
        float minX = buffer;
        float maxX = screenSize.X - buffer;
        float minY = buffer;
        float maxY = screenSize.Y - buffer - bottomUiHeight;

        for (int i = 0; i < 30; i++)
        {
            float x = (float)GD.RandRange(minX, maxX);
            float y = (float)GD.RandRange(minY, maxY);
            var candidate = new Vector2(x, y);
            
            if (candidate.DistanceTo(playerPos) >= minDistance)
                return candidate;
        }
        
        return new Vector2(
            (float)GD.RandRange(minX, maxX),
            (float)GD.RandRange(minY, maxY)
        );
    }
}