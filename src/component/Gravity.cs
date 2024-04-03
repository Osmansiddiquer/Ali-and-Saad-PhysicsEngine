using PhysicsEngine.src.physics._2D.body;
using Raylib_cs;

namespace PhysicsEngine.src.components;
public class Gravity : Component
{  
    public override void RunComponent(RigidBody2D body)
    {
        // Run the component
        if (body.IsOnGround)
        {
            System.Console.WriteLine(body.Name + "is On Ground");
        } else
        {
            ApplyGravity(body);
        }
    }

    public void ApplyGravity(RigidBody2D body)
    {
        body.LinVelocity.Y += 9.81f * Raylib.GetFrameTime();
        //System.Console.WriteLine("Gravity is being applied to" + body.Name);
    }
}
