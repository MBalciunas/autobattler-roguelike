using Godot;

public partial class PlayerAbilityResource(AbilityResource ability) : GodotObject
{
    public AbilityResource AbilityResource { get; set; } = ability;
    public int Level { get; set; } = 1;
    public int Copies { get; set; } = 0;

    public void AddCopy()
    {
        int threshold = Level switch
        {
            1 => 2,
            2 => 6,
            _ => int.MaxValue
        };

        if (Copies >= threshold)
        {
            Level++;
            Copies = 0;
        }
        else
        {
            Copies++;
        }
    }
}