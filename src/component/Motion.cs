using PhysicsEngine.src.physics._2D.body;
using Raylib_cs;
using System.Diagnostics;
using System.Numerics;

namespace PhysicsEngine.src.components;
internal class Motion : Component
{
    internal override void RunComponent(RigidBody2D body, double delta)
    {
        UseMotion(body, delta);
    }

    private void UseMotion(RigidBody2D body, double delta)
    {
        // Calculate acceleration based on the accumulated force and mass
        Vector2 acceleration = body.Force / body.Substance.Mass;

        // Update velocity based on acceleration and time
        body.LinVelocity += acceleration * (float)delta;

        // Set linear velocity to zero if its magnitude is less than the threshold
        if (body.LinVelocity.Length() <= (float)delta)
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

