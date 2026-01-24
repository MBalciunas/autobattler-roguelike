using System.Collections.Generic;
using System.Linq;
using AutoBattlerRoguelike.Scripts.Abilities;
using Godot;

public abstract partial class Enemy : Area2D
{
    [Export] private float moveSpeed = 50;
    [Export] private float currentMoveSpeed = 50;
    [Export] private float attackCooldown = 2;
    [Export] private float attackRange = 100;
    [Export] private float maxHealth;
    [Export] private float health;
    [Export] private PackedScene damageTakenUI;
    [Export] public float SeparationRadius = 32f;
    [Export] public float SeparationStrength = 1.2f;
    private TextureProgressBar healthBar;
    
    public Node2D poisonedEffect;
    private Area2D separationArea;
    public Node2D bleedingEffect;
    public Node2D burningEffect;
    private List<DamageOverTime> activeDots = new();
    protected Player player;
    private Timer attackTimer;
    private Timer dotsTimer;
    private Timer slowTimer;
    private Timer knockbackStunTimer;
    private bool isMoving = true;

    [Signal]
    public delegate void DiedEventHandler();


    public override void _Ready()
    {
        bleedingEffect = GetNode<Node2D>("BleedingEffect");
        poisonedEffect = GetNode<Node2D>("PoisonedEffect");
        burningEffect = GetNode<Node2D>("BurningEffect");
        healthBar =  GetNode<TextureProgressBar>("HealthBar");
        currentMoveSpeed = moveSpeed;

        int level = GlobalManager.Level;
        moveSpeed += Mathf.Pow(1.05f, level - 1);
        maxHealth *= Mathf.Pow(1.15f, level - 1);
        health = maxHealth;

        attackTimer = new Timer();
        dotsTimer = new Timer();
        slowTimer = new Timer();
        AddChild(dotsTimer);
        dotsTimer.WaitTime = 1f;
        dotsTimer.OneShot = false;
        dotsTimer.Timeout += TakeDotsDamage;
        dotsTimer.Start();
        AddChild(slowTimer);
        slowTimer.OneShot = true;
        slowTimer.Timeout += RemoveSlow;
        knockbackStunTimer = new Timer();
        AddChild(knockbackStunTimer);
        knockbackStunTimer.OneShot = true;
        knockbackStunTimer.Timeout += EnableMovement;
        AddChild(attackTimer);
        attackTimer.WaitTime = attackCooldown;
        attackTimer.OneShot = true;
        attackTimer.Start();
        player = GetNode<Player>("../../Player");

        AddToGroup("Enemies");

        separationArea = GetNode<Area2D>("SeparationArea");
    }

    public override void _Process(double delta)
    {
        healthBar.Value = health / maxHealth * 100;
        if (!isMoving) return;

        player ??= GetNode<Player>("../../Player");

        Vector2 separation = Vector2.Zero;

        foreach (var area in GetNode<Area2D>("SeparationArea").GetOverlappingAreas())
        {
            var other = area.GetParent<Enemy>();
            if (other == this || !other.IsInGroup("Enemies") || !other.isMoving) continue;

            Vector2 d = GlobalPosition - other.GlobalPosition;
            float dist = d.Length();
            separation += d.Normalized() * (1f / Mathf.Max(dist, 8f)) * 25;
        }

        Vector2 dir;

        if (separation.LengthSquared() > 0.25f)
        {
            dir = separation;
        }
        else
        {
            dir = Vector2.Zero;
            if (GlobalPosition.DistanceTo(player.GlobalPosition) > attackRange)
                dir = (player.GlobalPosition - GlobalPosition).Normalized();
        }

        if (dir.LengthSquared() > 0.001f)
            dir = dir.Normalized();

        Position += dir * currentMoveSpeed * (float)delta;

        if (GlobalPosition.DistanceTo(player.GlobalPosition) <= attackRange &&
            attackTimer.TimeLeft <= 0)
        {
            Attack();
            attackTimer.Start();
        }
    }

    private void TakeDotsDamage()
    {
        var dotsToRemove = new List<DamageOverTime>();
        foreach (var damageOverTime in activeDots)
        {
            damageOverTime.durationLeft -= 1f;
            TakeDamage(damageOverTime.damage * damageOverTime.stacks, damageOverTime.elementType, DamageType.DoT);
            if (damageOverTime.durationLeft <= 0) dotsToRemove.Add(damageOverTime);
        }

        dotsToRemove.ForEach(dot => activeDots.Remove(dot));
        
        if (activeDots.All(dot => dot.elementType != ElementType.Poison))
        {
            poisonedEffect.Hide();
        }

        if (activeDots.All(dot => dot.elementType != ElementType.Bleed))
        {
            bleedingEffect.Hide();
        }

        if (activeDots.All(dot => dot.elementType != ElementType.Fire))
        {
            burningEffect.Hide();
        }
    }

    public float TakeDamage(float damage, ElementType elementType = ElementType.None, DamageType damageType = DamageType.Direct)
    {
        if (damageType == DamageType.Direct)
        {
            damage *= (1 + player.playerState.Damage.Value / 100);
            var critRoll = GD.Randf() * 100;
            if (critRoll <= player.playerState.CritChance.Value)
            {
                damage *= (player.playerState.CritDamage.Value / 100);
            }

            if (player.playerState.Lifesteal.Value > 0)
            {
                var healAmount = player.playerState.Lifesteal.Value * damage / 100f;
                player.Heal(healAmount);
            }
        }

        health -= damage;
        var damageTakenEffect = damageTakenUI.Instantiate<DamageTakenUI>();
        damageTakenEffect.Position = GlobalPosition;
        GetTree().Root.GetNode("MainLevel").AddChild(damageTakenEffect);
        damageTakenEffect.Init(damage, elementType);
        if (health <= 0)
        {
            RemoveFromGroup("Enemies");
            EmitSignal(SignalName.Died);

            CallDeferred("queue_free");
        }

        return damage;
    }

    public void Knockback(float knockbackStrength, Vector2 direction)
    {
        isMoving = false;

        var tween = GetTree().CreateTween();
        var targetPosition = GlobalPosition + direction * knockbackStrength;

        tween.TweenProperty(this, "global_position", targetPosition, 0.4f).SetEase(Tween.EaseType.Out);
        tween.TweenCallback(Callable.From(StartKnockbackStun));
    }

    private void StartKnockbackStun()
    {
        knockbackStunTimer.WaitTime = 0.5f;
        knockbackStunTimer.Start();
    }

    private void EnableMovement()
    {
        isMoving = true;
    }

    public abstract void Attack();

    public void AddActiveDot(DamageOverTime damageOverTime)
    {
        // SerpentEffect.ApplyToDot(damageOverTime);
        var existingDot = activeDots.FirstOrDefault(dot => dot.elementType == damageOverTime.elementType);
        if (existingDot != null)
        {
            existingDot.stacks += damageOverTime.stacks;
            existingDot.ResetDuration();
        }
        else
        {
            activeDots.Add(damageOverTime);
        }

        if (activeDots.Any(dot => dot.elementType == ElementType.Poison))
        {
            poisonedEffect.Show();
        }

        if (activeDots.Any(dot => dot.elementType == ElementType.Bleed))
        {
            bleedingEffect.Show();
        }

        if (activeDots.Any(dot => dot.elementType == ElementType.Fire))
        {
            burningEffect.Show();
        }
    }

    public bool IsPoisoned()
    {
        return activeDots.Any(d => d.elementType == ElementType.Poison);
    }

    public bool IsBurning()
    {
        return activeDots.Any(d => d.elementType == ElementType.Fire);
    }

    public void AddSlow(float slow, float slowDuration)
    {
        currentMoveSpeed = moveSpeed * (1 - slow);
        slowTimer.WaitTime = slowDuration;
        slowTimer.Start();
    }

    private void RemoveSlow()
    {
        currentMoveSpeed = moveSpeed;
    }
}