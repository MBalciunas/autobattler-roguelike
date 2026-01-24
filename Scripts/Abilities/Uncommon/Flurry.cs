using System.Linq;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Uncommon;

public partial class Flurry : Ability
{
    [Export] private PackedScene flurryProjectileScene;
    private const float FireInterval = 0.08f;

    protected override async void ExecuteAbility()
    {
        var enemies = GlobalManager.GetEnemiesSortedByClosest();
        if (enemies.Count == 0) return;

        var stats = GetStats();
        int projectileCount = stats.projectileCount > 0 ? stats.projectileCount : 6;
        float damage = stats.damage;

        for (int i = 0; i < projectileCount; i++)
        {
            // Cycle through closest enemies
            var targetIndex = i % enemies.Count;
            var target = enemies[targetIndex];

            var projectile = flurryProjectileScene.Instantiate<FlurryProjectile>();
            projectile.Init(damage);
            projectile.GlobalPosition = GlobalPosition;
            var direction = (target.GlobalPosition - GlobalPosition).Normalized();
            projectile.Rotation = direction.Angle();
            GetTree().Root.GetNode("MainLevel").AddChild(projectile);

            await ToSignal(GetTree().CreateTimer(FireInterval), SceneTreeTimer.SignalName.Timeout);
        }
    }
}
