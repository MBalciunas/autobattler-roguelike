using System.Collections.Generic;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Uncommon;

public partial class StalkingPounce : Ability
{
    private const float JumpDuration = 0.2f;
    private const float BehindEnemyDistance = 150f;

    protected override async void ExecuteAbility()
    {
        List<Enemy> enemies = GlobalManager.GetEnemiesSortedByClosest();
        if (enemies.Count == 0) return;

        var stats = GetStats();

        // Get the furthest enemy (last in the sorted-by-closest list)
        var furthestEnemy = enemies[^1];

        // Calculate position behind the enemy (opposite side from player)
        var directionFromPlayer = (furthestEnemy.GlobalPosition - GlobalPosition).Normalized();
        var targetPosition = furthestEnemy.GlobalPosition + directionFromPlayer * BehindEnemyDistance;

        GlobalManager.Player.isInvulnerable = true;

        // Jump to position behind enemy
        var tween = GetTree().CreateTween();
        tween.TweenProperty(GlobalManager.Player, "global_position", targetPosition, JumpDuration);
        await ToSignal(tween, Tween.SignalName.Finished);

        GlobalManager.Player.isInvulnerable = false;

        // Set the damage bonus for the next ability
        GlobalManager.playerState.NextAbilityDamageBonus = stats.nextAbilityDamageBonus;
    }
}
