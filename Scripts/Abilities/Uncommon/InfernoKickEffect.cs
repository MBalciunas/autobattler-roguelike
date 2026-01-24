using System.Collections.Generic;
using Godot;

namespace AutoBattlerRoguelike.Scripts.Abilities.Uncommon;

public partial class InfernoKickEffect : Area2D
{
    private float damage;
    private int burnStacks;
    private readonly HashSet<Enemy> enemiesHit = new();

    private const float SnapshotWindow = 0.1f;
    private const float SnapshotInterval = 0.02f;

    public override async void _Ready()
    {
        Monitoring = true;
        Monitorable = true;

        float elapsed = 0f;
        while (elapsed < SnapshotWindow)
        {
            foreach (var a in GetOverlappingAreas())
            {
                if (a is Enemy e && enemiesHit.Add(e))
                {
                    e.TakeDamage(damage);
                    e.AddActiveDot(DamageOverTime.GetBurn(burnStacks));
                }
            }

            await ToSignal(GetTree().CreateTimer(SnapshotInterval), SceneTreeTimer.SignalName.Timeout);
            elapsed += SnapshotInterval;
        }

        FadeAndFree();
    }

    public override void _Draw()
    {
        var poly = GetNodeOrNull<CollisionPolygon2D>("CollisionPolygon2D");
        if (poly == null) return;

        var pts = poly.Polygon;
        if (pts == null || pts.Length < 3) return;

        var drawPts = new Vector2[pts.Length];
        for (int i = 0; i < pts.Length; i++)
            drawPts[i] = ToLocal(poly.ToGlobal(pts[i]));

        DrawColoredPolygon(drawPts, new Color(1, 0.5f, 0, 0.25f));

        for (int i = 0; i < drawPts.Length; i++)
        {
            var a = drawPts[i];
            var b = drawPts[(i + 1) % drawPts.Length];
            DrawLine(a, b, new Color(1, 0.5f, 0, 0.9f), 2f);
        }
    }

    public override void _Process(double delta)
    {
        QueueRedraw();
    }

    private async void FadeAndFree()
    {
        var tween = GetTree().CreateTween();
        tween.TweenProperty(this, "modulate", new Color(0, 0, 0, 0), 0.5f);
        await ToSignal(tween, Tween.SignalName.Finished);
        CallDeferred("queue_free");
    }

    public void Init((float damage, int burnStacks) stats)
    {
        damage = stats.damage;
        burnStacks = stats.burnStacks;
    }
}
