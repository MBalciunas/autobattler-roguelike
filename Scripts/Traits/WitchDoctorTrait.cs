using AutoBattlerRoguelike.Scripts.Abilities;

namespace AutoBattlerRoguelike.Scripts.Traits;

public static class WitchDoctorEffect 
{
    public static void ApplyToDot(DamageOverTime dot)
    {
        var traitCount = GlobalManager.playerState.GetTraitCount(AbilityTrait.Serpent);

        float damageMultiplier = traitCount switch
        {
            >= 6 => 2.5f,
            >= 3 => 1.5f,
            _ => 1.0f
        };
        
        float durationMultiplier = traitCount switch
        {
            >= 6 => 2.0f,
            >= 3 => 1.5f,
            _ => 1.0f
        };
        
        dot.damage *= damageMultiplier;
        dot.durationLeft *= durationMultiplier;
        
        //TODO Need to implement dots spreading on death for 6 traits
    }
}