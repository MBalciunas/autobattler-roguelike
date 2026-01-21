using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Rare;

public partial class ToxicPoolEffect : Area2D
{
    private float damage;
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
            CallDeferred("queue_free");
        }
    }
    
    private void OnAreaEntered(Area2D area)
    {
        if (area is Enemy enemy)
        {
            enemy.TakeDamage(damage);
            DamageOverTime.GetPoison(poisonStacks);
            enemy.AddSlow(slow, 1f);
        }
    }

    public void Init((float damage, int poisonStacks, float slow, float duration) stats)
    {
        damage = stats.damage;
        poisonStacks = stats.poisonStacks;
        duration = stats.duration;
        slow = stats.slow;
    }
}