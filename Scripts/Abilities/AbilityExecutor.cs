using Godot;
using Godot.Collections;

public partial class AbilityExecutor : Node2D
{
    private Array<Ability> activeAbilities = [];
    public int nextAbilityIndex = 0;
    private bool isNextAbilityReady = true;
    private float timeToLoop = 4.0f;
    public Timer cooldownTimer;

    public override void _Ready()
    {
        cooldownTimer = GetNode<Timer>("CooldownTimer");
        cooldownTimer.Timeout += NextReady;
        foreach (var playerAbility in GlobalManager.playerState.AbilitiesInLoop)
        {
            var ability = playerAbility.AbilityResource.abilityScene.Instantiate<Ability>();
            activeAbilities.Add(ability);
            ability.Level = playerAbility.Level;
            AddChild(ability);
            ability.GlobalPosition = GlobalPosition;
        }
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
        currentAbility.Finished -= StartAbilityCooldownTimer;
        cooldownTimer.WaitTime = timeToLoop / activeAbilities.Count;
        cooldownTimer.Start();
    }
    
    private void NextReady()
    {
        cooldownTimer.Stop();
        nextAbilityIndex = (nextAbilityIndex + 1) % activeAbilities.Count;
        isNextAbilityReady = true;
    }
}
