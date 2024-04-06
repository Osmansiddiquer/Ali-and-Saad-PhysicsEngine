using PhysicsEngine.src.physics._2D.body;
using Raylib_cs;
using System.Numerics;

namespace PhysicsEngine.src.components;
internal class Motion : Component
{
    internal override void RunComponent(RigidBody2D body)
    {
        UseMotion(body);
    }

    private void UseMotion(RigidBody2D body)
    {
        // Calculate acceleration based on the accumulated force and mass
        Vector2 acceleration = body.Force / body.Substance.Mass;

        // Update velocity based on acceleration and time
        body.LinVelocity += acceleration * (Raylib.GetFrameTime() / 400);

        // Set linear velocity to zero if its magnitude is less than the threshold
        if (body.LinVelocity.Length() <= (Raylib.GetFrameTime() / 400))
        {
            body.LinVelocity = Vector2.Zero;
        }

        // Update position based on linear velocity
        body.Translate(body.LinVelocity);
        body.Rotate(body.RotVelocity);

        body.VerticesUpdateRequired = true;
        body.AABBUpdateRequired = true;

        // Reset force applied to the body
        body.Force = Vector2.Zero;
    }
}

