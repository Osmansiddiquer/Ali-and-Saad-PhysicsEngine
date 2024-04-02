using PhysicsEngine.src.physics._2D.body;
using Raylib_cs;

namespace PhysicsEngine.src.components;
public class Gravity : Component
{  
    public override void RunComponent(RigidBody2D body)
    {
        if (!body.IsOnFloor)
        {
            ApplyGravity(body);
        }
    }

    private void ApplyGravity(RigidBody2D body)
    {
        body.LinVelocity.Y += 9.81f * Raylib.GetFrameTime();
    }
}
