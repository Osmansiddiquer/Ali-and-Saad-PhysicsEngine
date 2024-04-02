using PhysicsEngine.src.body;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysicsEngine.src.components;

namespace PhysicsEngine.src.physics._2D.body;
public class PlayerBody : RigidBody2D {

    Camera2D camera;
    public PlayerBody(RigidBody2D body, Camera2D camera) : base(body)
    {
        this.camera = camera;
        components.Add(new PlayerInput());
    }

    public PlayerBody(RigidBody2D body) : base(body)
    {
        components.Add(new PlayerInput());
    }
}

