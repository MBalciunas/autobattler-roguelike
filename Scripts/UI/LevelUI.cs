using Godot;

public partial class LevelUI : Control
{
    [Export] private EnemySpawner enemySpawner;
    private Label waveLabel;
    private Label roundLabel;

    private void UpdateRound(int round)
    {
        roundLabel.Text = $"Round {round}";
    }

    private void UpdateWave(int currentWave, int totalWaves)
    {
        waveLabel.Text = $"Wave: {currentWave} / {totalWaves}";
    }

    public override void _EnterTree()
    {
        waveLabel = GetNode<Label>("Wave");
        roundLabel = GetNode<Label>("Round");
        UpdateRound(GlobalManager.Level);
        enemySpawner.OnWaveNumberChanged += UpdateWave;
    }

    public override void _ExitTree()
    {
        enemySpawner.OnWaveNumberChanged -= UpdateWave;
    }
}
