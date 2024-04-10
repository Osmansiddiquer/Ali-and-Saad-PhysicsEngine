using GameEngine.src.physics.body;
using System.Numerics;

namespace GameEngine.src.physics.component;
public class Motion : Component
{
    internal override void RunComponent(RigidBody2D body, double delta)
    {
        UseMotion(body, delta);
    }

    private void UseMotion(RigidBody2D body, double delta)
    {
        // Calculate acceleration based on the accumulated force and mass
        Vector2 acceleration = body.Force / body.Material.Mass;

        // Update velocity based on acceleration and time
        body.LinVelocity += acceleration * (float)delta;

        // Set linear velocity to zero if its magnitude is less than the threshold
        if (body.LinVelocity.Length() <= (float)delta)
            body.LinVelocity = Vector2.Zero;


        // Update position based on linear velocity
        body.Translate(body.LinVelocity);
        body.Rotate(body.RotVelocity);

        // Reset force applied to the body
        body.Force = Vector2.Zero;
    }
}

