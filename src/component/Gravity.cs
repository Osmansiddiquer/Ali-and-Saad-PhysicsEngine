using PhysicsEngine.src.physics._2D.body;
using Raylib_cs;
using System.Numerics;

namespace PhysicsEngine.src.components;
internal class Gravity : Component
{  
    internal override void RunComponent(RigidBody2D body)
    {
        // Move body downwards if it is midair
        if (!(body.Normal.Y == 1f) || body is ProjectileBody2D) { ApplyGravity(body); }
    }

    // Calculate and apply gravitational acceleration
    private void ApplyGravity(RigidBody2D body)
    {
        Vector2 gravity = new Vector2(0, 9.81f * body.Substance.Mass);
        body.ApplyForce(gravity);
    }
}
