using AutoBattlerRoguelike.Scripts;
using AutoBattlerRoguelike.Scripts.Abilities;
using Godot;

public partial class Player : Area2D
{
    [Export] public PlayerState playerState;

    public bool isInvulnerable = false;
    public bool isCharging = false;
    private ChargingEffect chargingEffect;

    public override void _Process(double delta)
    {
        if (isCharging)
        {
            var screenRect = GetViewport().GetVisibleRect();

            float bottomUiHeight = 150f;
            float buffer = 15f;

            screenRect.Position += new Vector2(buffer, buffer);
            screenRect.Size -= new Vector2(
                buffer * 2,
                buffer * 2 + bottomUiHeight
            );

            if (!screenRect.HasPoint(GlobalPosition))
            {
                if (GlobalPosition.X < screenRect.Position.X || GlobalPosition.X > screenRect.End.X)
                    chargingEffect.Direction.X *= -1;

                if (GlobalPosition.Y < screenRect.Position.Y|| GlobalPosition.Y > screenRect.End.Y)
                    chargingEffect.Direction.Y *= -1;
            }

            var distanceTraveled = chargingEffect.Direction * (float)delta * 1500f;
            chargingEffect.Distance -= distanceTraveled.Length();
            GlobalPosition += distanceTraveled;


            if (chargingEffect.Distance <= 0)
            {
                StopCharging();
                chargingEffect = null;
            }
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is Enemy enemy)
        {
            if (isCharging)
            {
                enemy.TakeDamage(chargingEffect.Damage);
                enemy.Knockback(chargingEffect.KnockbackStrength, GlobalPosition.DirectionTo(enemy.GlobalPosition));
                enemy.AddActiveDot(chargingEffect.DotOnContact);
            }
        }
    }


    public void StartCharging(
        Vector2 direction,
        float distance,
        float damageOnContact,
        float knockbackStrength,
        DamageOverTime dotOnContact
    )
    {
        chargingEffect = new ChargingEffect(direction, distance, damageOnContact, knockbackStrength, dotOnContact);
        isCharging = true;
        isInvulnerable = true;
    }

    private void StopCharging()
    {
        isCharging = false;
        isInvulnerable = false;
    }

    public void TakeDamage(float damage)
    {
        if (isInvulnerable) return;

        float armor = playerState.Armor.Value;
        float percentReduced = damage * (100f / (100f + armor));
        float finalDamage = Mathf.Max(1f, percentReduced - armor * 0.05f); // Mix of % and flat reduction

        playerState.TakeDamage(finalDamage);
    }

    public void Heal(float healAmount)
    {
        playerState.Heal(healAmount);
    }
}

public class ChargingEffect(
    Vector2 direction,
    float distance,
    float knockbackStrength,
    float damage,
    DamageOverTime dotOnContact)
{
    public Vector2 Direction = direction;
    public float Distance = distance;
    public float KnockbackStrength = knockbackStrength;
    public float Damage = damage;
    public DamageOverTime DotOnContact = dotOnContact;
}