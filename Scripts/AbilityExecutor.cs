using Godot;
using Godot.Collections;

public partial class AbilityExecutor : Node2D
{
    [Export] private Array<Ability> activeAbilities = [];
    private int nextAbilityIndex = 0;
    private bool isNextAbilityReady = true;
    private float timeToLoop = 3.0f;
    private Timer abilityCooldownTimer;

    public override void _Ready()
    {
        abilityCooldownTimer = GetNode<Timer>("CooldownTimer");
        abilityCooldownTimer.Timeout += NextAbilityReady;
    }

    public override void _Process(double delta)
    {
        if (isNextAbilityReady)
        {
            isNextAbilityReady = false;
            var currentAbility = activeAbilities[nextAbilityIndex];
            currentAbility.Finished += StartAbilityCooldownTimer;
            currentAbility.Execute();
        }
    }

    private void StartAbilityCooldownTimer(Ability currentAbility)
    {
        GD.Print("Starting cooldown timer");
        currentAbility.Finished -= StartAbilityCooldownTimer;
        abilityCooldownTimer.WaitTime = timeToLoop / activeAbilities.Count;
        abilityCooldownTimer.Start();
    }
    
    private void NextAbilityReady()
    {
        abilityCooldownTimer.Stop();
        GD.Print("Next Ability Ready");
        nextAbilityIndex = (nextAbilityIndex + 1) % activeAbilities.Count;
        isNextAbilityReady = true;
    }
}
