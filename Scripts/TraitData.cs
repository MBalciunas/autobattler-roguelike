using System.Collections.Generic;
using Godot;

namespace AutoBattlerRoguelike.Scripts;

public partial class TraitData : Resource
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<TraitTier> Tiers { get; set; }
}

public class TraitTier
{
    public int Required { get; set; }
    public string Effect { get; set; }
}

public class TraitJson
{
    public string name { get; set; }
    public string description { get; set; }
    public List<TraitTierJson> tiers { get; set; }
}

public class TraitTierJson
{
    public int required { get; set; }
    public string effect { get; set; }
}
