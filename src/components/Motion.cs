using PhysicsEngine.src.body;
using Raylib_cs;
using System.Numerics;

namespace PhysicsEngine.src.components;
public class Motion : Component
{
    public override void RunComponent(RigidBody2D body)
    {
        UseMotion(body);
    }

    public void UseMotion(RigidBody2D body)
    {
        body.LinVelocity += (body.Force / body.Substance.Mass) * Raylib.GetFrameTime();

        body.Transform.Translate(body.LinVelocity);
        body.Transform.Rotate(body.RotVelocity);

        body.VerticesUpdateRequired = true;
        body.AABBUpdateRequired = true;

        body.Force = Vector2.Zero;
    }
}

