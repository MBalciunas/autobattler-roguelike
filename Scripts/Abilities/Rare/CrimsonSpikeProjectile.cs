using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Rare;

public partial class CrimsonSpikeProjectile : Area2D
{
    private int bleedStacks;
    private float speed = 600;

    public override void _Process(double delta)
    {
        Vector2 forward = Vector2.Right.Rotated(Rotation);
        GlobalPosition += forward * speed * (float)delta;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is Enemy enemy)
        {
            enemy.AddActiveDot(DamageOverTime.GetBleed(bleedStacks));
        }
    }

    public void Init(int bleedStacks)
    {
        this.bleedStacks = bleedStacks;
    }
}