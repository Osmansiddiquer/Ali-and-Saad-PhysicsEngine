using PhysicsEngine.src.body;

namespace PhysicsEngine.src.components;
public class Gravity : Component
{
    public override void RunComponent(RigidBody2D body)
    {
        // Run the component
        ApplyGravity(body);
    }
    public void ApplyGravity(RigidBody2D body)
    {
        body.LinVelocity.Y += 9.81f / 60;
    }
}
