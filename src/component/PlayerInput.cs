//using PhysicsEngine.src.physics._2D.body;
//using Raylib_cs;


//namespace PhysicsEngine.src.components;
//public class PlayerInput : Component {
//    Camera2D camera;
//    public override void RunComponent(RigidBody2D body)
//    {
//        // Run the component
//        ApplyInput(body);

//    }

//    public void ApplyInput(RigidBody2D body)
//    {
//        if (Raylib.IsKeyDown(KeyboardKey.Up))
//        {
//            body.LinVelocity.Y -= 0.1f;
//            Console.WriteLine("Up");
//            Raylib.EndMode2D();
//        }
//        if (Raylib.IsKeyDown(KeyboardKey.Down))
//        {
//            body.LinVelocity.Y += 0.1f;
//        }
//        if (Raylib.IsKeyDown(KeyboardKey.Left))
//        {
//            body.LinVelocity.X -= 0.1f;
//        }
//        if (Raylib.IsKeyDown(KeyboardKey.Right))
//        {
//            body.LinVelocity.X += 0.1f;
//        }
//    }
//}

