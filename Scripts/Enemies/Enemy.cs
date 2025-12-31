using System;
using System.Collections.Generic;
using AutoBattlerRoguelike.Scripts.Abilities;
using AutoBattlerRoguelike.Scripts.Traits;
using Godot;

public abstract partial class Enemy : Area2D
{
    [Export] private float moveSpeed = 50;
    [Export] private float attackCooldown = 2;
    [Export] private float attackRange = 100;
    [Export] private float health = 2;
    private List<DamageOverTime> activeDots = new();
    protected Player player;
    private Timer attackTimer;
    private Timer dotsTimer;
    private bool isMoving = true;

    public override void _Ready()
    {
        attackTimer = new Timer();
        dotsTimer = new Timer();
        AddChild(dotsTimer);
        dotsTimer.WaitTime = 1f;
        dotsTimer.OneShot = false;
        dotsTimer.Timeout += TakeDotsDamage;
        dotsTimer.Start();
        AddChild(attackTimer);
        attackTimer.WaitTime = attackCooldown;
        attackTimer.OneShot = true;
        attackTimer.Start();
        player = GetNode<Player>("../../Player");
        
        AddToGroup("Enemies");
    }

    public override void _Process(double delta)
    {
        if(!isMoving) return;
        
        if (attackRange >= GlobalPosition.DistanceTo(player.GlobalPosition))
        {
            if (attackTimer.TimeLeft <= 0)
            {
                Attack();
                attackTimer.Start();
            }
        }
        else
        {
            player = GetNode<Player>("../../Player");
            var direction = (player.GlobalPosition - GlobalPosition).Normalized();
            Position += direction * moveSpeed * (float)delta;
        }
    }

    private void TakeDotsDamage()
    {
        var dotsToRemove = new List<DamageOverTime>();
        foreach (var damageOverTime in activeDots)
        {
            damageOverTime.durationLeft -= 1f;
            TakeDamage(damageOverTime.damage, DamageType.DoT);
            if (damageOverTime.durationLeft <= 0) dotsToRemove.Add(damageOverTime);
        }

        dotsToRemove.ForEach(dot => activeDots.Remove(dot));
    }
    
    public void TakeDamage(float damage, DamageType damageType = DamageType.Direct)
    {
        if (damageType == DamageType.Direct)
        {
            var critRoll = GD.Randf() * 100;
            if (critRoll <= player.playerState.CritChance.Value)
            {
                damage *= player.playerState.CritDamage.Value;
            }

            if (player.playerState.Lifesteal.Value > 0)
            {
                var healAmount = player.playerState.Lifesteal.Value * damage;
                player.Heal(healAmount);
            }
        }
        
        health -= damage;
        if (health <= 0)
        {
            RemoveFromGroup("Enemies");
            if (GetTree().GetNodesInGroup("Enemies").Count == 0 && !GlobalManager.IsEnemiesSpawning)
            {
                GameManager.Instance.FinishLevel();
            }

            CallDeferred("queue_free");
        }
    }

    public void Knockback(float knockbackStrength, Vector2 direction)
    {
        isMoving = false;
        var tween = GetTree().CreateTween();
        var targetPosition = GlobalPosition + direction * knockbackStrength;

        tween.TweenProperty(this, "global_position", targetPosition, 0.3f).SetEase(Tween.EaseType.Out);
        tween.Finished += EnableMovement;
    }

    private void EnableMovement()
    {
        isMoving = true;
    }
    
    public abstract void Attack();

    public void AddActiveDot(DamageOverTime damageOverTime)
    {
        WitchDoctorEffect.ApplyToDot(damageOverTime);
        activeDots.Add(damageOverTime);
        GD.Print("Dots damage: " + damageOverTime.damage);
        GD.Print("Dots durations: " + damageOverTime.durationLeft);
    }
}