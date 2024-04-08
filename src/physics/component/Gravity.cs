using GameEngine.src.physics.body;
using System.Numerics;

namespace GameEngine.src.physics.component;
internal class Gravity : Component
{
    internal override void RunComponent(RigidBody2D body, double delta)
    {
        // Move body downwards if it is midair
        ApplyGravity(body);
    }

    // Calculate and apply gravitational acceleration
    private void ApplyGravity(RigidBody2D body)
    {
        Vector2 gravity = new Vector2(0, 9.81f * body.Material.Mass);
        body.ApplyForce(gravity);
    }
}
