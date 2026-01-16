using AutoBattlerRoguelike.Scripts;
using Godot;

public partial class PlayerStatsUI : VBoxContainer
{
    private Label _maxHealth;
    private Label _armor;
    private Label _damage;
    private Label _critChance;
    private Label _critDamage;
    private Label _lifesteal;
    private Label _poisonDamage;
    private Label _bleedDamage;

    private PlayerState _playerState; 

    public override void _Ready()
    {
        _playerState = GlobalManager.playerState;

        _maxHealth = GetNode<Label>("MaxHealth");
        _armor = GetNode<Label>("Armor");
        _damage = GetNode<Label>("Damage");
        _critChance = GetNode<Label>("CritChance");
        _critDamage = GetNode<Label>("CritDamage");
        _lifesteal = GetNode<Label>("Lifesteal");
        _poisonDamage = GetNode<Label>("PoisonDamage");
        _bleedDamage = GetNode<Label>("BleedDamage");

        _playerState.MaxHealth.OnValueChanged += OnMaxHealthChanged;
        _playerState.Armor.OnValueChanged += OnArmorChanged;
        _playerState.Damage.OnValueChanged += OnDamageChanged;
        _playerState.CritChance.OnValueChanged += OnCritChanceChanged;
        _playerState.CritDamage.OnValueChanged += OnCritDamageChanged;
        _playerState.Lifesteal.OnValueChanged += OnLifestealChanged;
        _playerState.PoisonDamage.OnValueChanged += OnPoisonDamageChanged;
        _playerState.BleedDamage.OnValueChanged += OnBleedDamageChanged;

        OnMaxHealthChanged(_playerState.MaxHealth.Value);
        OnArmorChanged(_playerState.Armor.Value);
        OnDamageChanged(_playerState.Damage.Value);
        OnCritChanceChanged(_playerState.CritChance.Value);
        OnCritDamageChanged(_playerState.CritDamage.Value);
        OnLifestealChanged(_playerState.Lifesteal.Value);
    }

    private void OnMaxHealthChanged(float value) => _maxHealth.Text = $"Max Health: {value:0.##}";
    private void OnArmorChanged(float value) => _armor.Text = $"Armor: {value:0.##}";
    private void OnDamageChanged(float value) => _damage.Text = $"Damage %: {value:0.##}";
    private void OnCritChanceChanged(float value) => _critChance.Text = $"Crit Chance %: {value:0.#}";
    private void OnCritDamageChanged(float value) => _critDamage.Text = $"Crit Damage %: {value:0.#}";
    private void OnLifestealChanged(float value) => _lifesteal.Text = $"Lifesteal %: {value:0.#}";
    private void OnPoisonDamageChanged(float value) => _poisonDamage.Text = $"Poison Damage %: {value:0.#}";
    private void OnBleedDamageChanged(float value) => _bleedDamage.Text = $"Bleed Damage %: {value:0.#}";

    public override void _ExitTree()
    {
        _playerState.MaxHealth.OnValueChanged -= OnMaxHealthChanged;
        _playerState.Armor.OnValueChanged -= OnArmorChanged;
        _playerState.Damage.OnValueChanged -= OnDamageChanged;
        _playerState.CritChance.OnValueChanged -= OnCritChanceChanged;
        _playerState.CritDamage.OnValueChanged -= OnCritDamageChanged;
        _playerState.Lifesteal.OnValueChanged -= OnLifestealChanged;
        _playerState.PoisonDuration.OnValueChanged -= OnPoisonDamageChanged;
        _playerState.BleedDamage.OnValueChanged -= OnBleedDamageChanged;
    }
}