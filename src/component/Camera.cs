using Raylib_cs;


namespace PhysicsEngine.src.components;
public class Camera : Component
{
    Camera2D camera;
    public Camera()
    {
        
    }
    public override void RunComponent(RigidBody2D body)
    {
        UpdateCamera(body);
    }

    private void UpdateCamera(RigidBody2D body)
    {
        camera.Target = body.Transform.Position;
    }
}

