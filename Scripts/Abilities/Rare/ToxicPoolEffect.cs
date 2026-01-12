using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Rare;

public partial class ToxicPoolEffect : Area2D
{
    private float damage;
    private float poisonDamage;
    private int poisonStacks;
    private float duration;
    private int timePassed = 0;
    private float slow;
    private Timer effectTimer;

    
    public override async void _Ready()
    {
        effectTimer = new Timer();
        AddChild(effectTimer);
        effectTimer.WaitTime = 1f;
        effectTimer.OneShot = false;
        effectTimer.Timeout += ApplyEffect;
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
        effectTimer.Start();
    }

    private async void ApplyEffect()
    {
        foreach (var area in GetOverlappingAreas())
        {
            if (area is Enemy enemy)
            {
                enemy.TakeDamage(damage);
                enemy.AddActiveDot(DamageOverTime.GetPoison(poisonStacks));
                enemy.AddSlow(slow, 1f);
            }
        }

        timePassed++;
        if (timePassed > duration)
        {
            var tween = GetTree().CreateTween();
            tween.TweenProperty(this, "modulate", new Color(0, 0, 0, 0), 0.3);
            await ToSignal(tween, Tween.SignalName.Finished);
            QueueFree();
        }
    }
    
    private void OnAreaEntered(Area2D area)
    {
        if (area is Enemy enemy)
        {
            enemy.TakeDamage(damage);
            enemy.AddActiveDot(new DamageOverTime(poisonDamage, poisonStacks, ElementType.Poison));
            enemy.AddSlow(slow, 1f);
        }
    }

    public void Init((float damage, int poisonDamage, float slow, float duration) stats)
    {
        damage = stats.damage;
        poisonDamage = stats.poisonDamage;
        duration = stats.duration;
        slow = stats.slow;
    }
}