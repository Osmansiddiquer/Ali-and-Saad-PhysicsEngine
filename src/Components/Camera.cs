using PhysicsEngine.src.body;
using Raylib_cs;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

