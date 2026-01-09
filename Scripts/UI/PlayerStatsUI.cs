using Godot;

public partial class PlayerStatsUI : VBoxContainer
{
    private Label maxHealth;
    private Label armor;
    private Label damage;
    private Label critChance;
    private Label critDamage;
    private Label lifesteal;

    public override void _Ready()
    {
        maxHealth = GetNode<Label>("MaxHealth");
        armor = GetNode<Label>("Armor");
        damage = GetNode<Label>("Damage");
        critChance = GetNode<Label>("CritChance");
        critDamage = GetNode<Label>("CritDamage");
        lifesteal = GetNode<Label>("Lifesteal");

        SetupListeners();
        maxHealth.Text = $"Max Health: {GlobalManager.playerState.MaxHealth.Value}";
        armor.Text = $"Armor: {GlobalManager.playerState.Armor.Value}";
        damage.Text = $"Damage %: {GlobalManager.playerState.Damage.Value}";
        critChance.Text = $"Crit Chance %: {GlobalManager.playerState.CritChance.Value}";
        critDamage.Text = $"Crit Damage %: {GlobalManager.playerState.CritDamage.Value}";
        lifesteal.Text = $"Lifesteal %: {GlobalManager.playerState.Lifesteal.Value}";
    }

    private void SetupListeners()
    {
        GlobalManager.playerState.MaxHealth.OnValueChanged += (value) => maxHealth.Text = $"Max Health: {value}";
        GlobalManager.playerState.Armor.OnValueChanged += (value) => armor.Text = $"Armor: {value}";
        GlobalManager.playerState.Damage.OnValueChanged += (value) => damage.Text = $"Damage %: {value}";
        GlobalManager.playerState.CritChance.OnValueChanged += (value) => critChance.Text = $"Crit Chance %: {value}";
        GlobalManager.playerState.CritDamage.OnValueChanged += (value) => critDamage.Text = $"Crit Damage %: {value}";
        GlobalManager.playerState.Lifesteal.OnValueChanged += (value) => lifesteal.Text = $"Lifesteal %: {value}";
    }
}
