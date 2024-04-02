using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using PhysicsEngine.src.body;
using Raylib_cs;


namespace PhysicsEngine.src.components;
public class PlayerInput : Component {
    Camera2D camera;
    public override void RunComponent(RigidBody2D body)
    {
        // Run the component
        ApplyInput(body);

        //camera.Target = body.Transform.Position;
        //Raylib.BeginMode2D(camera);
    }

    //public PlayerInput(Camera2D camera) {
    //    this.camera = camera;
    //}

    public void ApplyInput(RigidBody2D body)
    {
        if (Raylib.IsKeyDown(KeyboardKey.Up))
        {
            body.LinVelocity.Y -= 0.1f;
            Console.WriteLine("Up");
            Raylib.EndMode2D();
        }
        if (Raylib.IsKeyDown(KeyboardKey.Down))
        {
            body.LinVelocity.Y += 0.1f;
        }
        if (Raylib.IsKeyDown(KeyboardKey.Left))
        {
            body.LinVelocity.X -= 0.1f;
        }
        if (Raylib.IsKeyDown(KeyboardKey.Right))
        {
            body.LinVelocity.X += 0.1f;
        }
    }
}

