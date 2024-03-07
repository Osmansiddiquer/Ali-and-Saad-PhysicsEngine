using PhysicsEngine.src.body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

public class GravityComponent : Component
{
    public override void runComponent(RigidBody2D body)
    {
        // Run the component
        applyGravity(body);
    }
    public void applyGravity(RigidBody2D body)
    {
        // Apply gravity to all objects
        body.ApplyForce(new Vector2(0, 9.81f));
    }
}
