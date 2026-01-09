using System.Collections.Generic;
using System.Linq;
using Godot;
using static UpgradablePlayerStat;

public partial class RoundEndSelection : Control
{
    private Label roundFinishedLabel;
    private Label goldEarnedLabel;
    private Label xpGainedLabel;
    private List<Control> statSelections;
    private List<Dictionary<UpgradablePlayerStat, float>> stats = new();

    public override void _Ready()
    {
        roundFinishedLabel = GetNode<Label>("RoundFinishedLabel");
        goldEarnedLabel = GetNode<Label>("GoldEarnedLabel");
        xpGainedLabel = GetNode<Label>("XpGainedLabel");
        statSelections = GetNode<Control>("StatsSelectionContainer").GetChildren().Select(c => c as Control).ToList();
        
        for (int i = 0; i < 3; i++)
        {
            int index = i;
            statSelections[i].GetNode<Button>("Button").Pressed +=() => AddStats(index);
        }
    }

    private void AddStats(int i)
    {
        var statsChosen = stats[i];

        foreach (var stat in statsChosen)
        {
            switch (stat.Key)
            {
                case Armor:
                    GlobalManager.playerState.Armor.Add(stat.Value);
                    break;
                case MaxHealth:
                    GlobalManager.playerState.MaxHealth.Add(stat.Value);
                    break;
                case CritChance:
                    GlobalManager.playerState.CritChance.Add(stat.Value);
                    break;
                case CritDamage:
                    GlobalManager.playerState.CritDamage.Add(stat.Value);
                    break;
                case Damage:
                    GlobalManager.playerState.Damage.Add(stat.Value);
                    break;  
                case Lifesteal:
                    GlobalManager.playerState.Lifesteal.Add(stat.Value);
                    break;
            }
        }
        
        GameManager.Instance.StatsChosen();
    }

    public void Update()
    {
        goldEarnedLabel.Text = $"Gold gained: {GlobalManager.CalculateGoldAfterRound()} + {GlobalManager.CalculateInterest()} Interest";
        xpGainedLabel.Text = $"Xp Gained: {GlobalManager.CalculateXpGain()}";
        roundFinishedLabel.Text = $"Round {GlobalManager.Level} Completed";
        
        stats.Clear();
        stats.Add(GlobalManager.RollRandomStatsSelection());
        stats.Add(GlobalManager.RollRandomStatsSelection());
        stats.Add(GlobalManager.RollRandomStatsSelection());

        for (int i = 0; i < 3; i++)
        {
            string result = string.Join(
                "\n",
                stats[i].Select(s =>
                    $"+{s.Value:0.##}{s.Key switch
                    {
                        CritChance or
                            CritDamage or
                            Lifesteal or
                            Damage => "%",
                        _ => ""
                    }} {s.Key}"
                )
            );
            
            statSelections[i].GetNode<Label>("Label").Text = result;
        }
    }
}