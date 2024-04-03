using PhysicsEngine.src.physics._2D.body;
using Raylib_cs;
using System.Numerics;

namespace PhysicsEngine.src.components;
public class LinMotion : Component
{
    public override void RunComponent(RigidBody2D body)
    {
        UseMotion(body);
    }

    public void UseMotion(RigidBody2D body)
    {
        Vector2 deltaVelocity = body.LinVelocity + (body.Force / body.Substance.Mass) * Raylib.GetFrameTime();

        // Set linear velocity to zero if its magnitude is less than the threshold
        body.LinVelocity = deltaVelocity.Length() < 0.1f ? Vector2.Zero : deltaVelocity;

        body.Transform.Translate(body.LinVelocity);

        body.VerticesUpdateRequired = true;
        body.AABBUpdateRequired = true;

        body.Force = Vector2.Zero;
    }
}

