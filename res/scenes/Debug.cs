using PhysicsEngine.src.body;
using PhysicsEngine.src.main;
using PhysicsEngine.src.physics;
using PhysicsEngine.src.physics._2D.body;
using PhysicsEngine.src.world;
using Raylib_cs;
using System.Numerics;

namespace PhysicsEngine.res.scenes;

public class Debug : PhysicsWorld2D
{
    // Member variables
    private static List<Color> colors;

    private static List<PhysicsBody2D> bodies;

    private static List<Vector2> positions;
    private static float rotation = 1f;
    private static Vector2 force = Vector2.Zero;

    // Constructor for initialization
    static Debug()
    {
        colors = new List<Color>() { Color.Red, Color.White, Color.White };

        bodies = new List<PhysicsBody2D>();
        positions = new List<Vector2>() { new Vector2(64, 320), new Vector2(480, 320), new Vector2(480, 600) };
    }

    // Ready function (Runs on first frame)
    public static void Ready()
    {
        Properties.DisplayFPS = true;
    }

    // Update function (Runs on every frame)
    public static void Update(double delta)
    {
        Draw();
        Collisions.HandleCollision(bodies);
        foreach (PhysicsBody2D body in bodies)
        {
            if (body is RigidBody2D)
            {
                RigidBody2D rigidBody = (RigidBody2D)body;
                rigidBody.process();
            }
        } 

        // Move circles based on input
        float dx = 0f;
        float dy = 0f;
        float magnitude = 30000f;

        if (Raylib.IsKeyDown(KeyboardKey.Left)) dx--;
        else if (Raylib.IsKeyDown(KeyboardKey.Right)) dx++;

        if (Raylib.IsKeyDown(KeyboardKey.Up)) dy--;
        else if (Raylib.IsKeyDown(KeyboardKey.Down)) dy++;

        float xAxis = Raylib.GetGamepadAxisMovement(0, GamepadAxis.LeftX);
        float yAxis = Raylib.GetGamepadAxisMovement(0, GamepadAxis.LeftY);

        if (Raylib.IsGamepadAvailable(0))
        {
            Vector2 direction = new Vector2(xAxis, yAxis) * (float)delta;
            force = direction * magnitude;

            RigidBody2D body = (RigidBody2D)bodies[0];
            body.ApplyForce(force);
        }

        else
        {
            if (dx != 0 || dy != 0)
            {
                Vector2 direction = Vector2.Normalize(new Vector2(dx, dy)) * (float)delta;
                force = direction * magnitude;

                if (bodies[0] is RigidBody2D)
                {
                    RigidBody2D body = (RigidBody2D)bodies[0];
                    body.ApplyForce(force);
                }
            }            
        }


        for (int i = 0; i < 2; i++)
        {
            if (bodies[i] is RigidBody2D)
            {
                RigidBody2D body = (RigidBody2D)bodies[i];
                body.Motion();
            }
        }

    }

    // Draw
    public static void Draw()
    {
        // Ensure bodies are created (call once or in Ready)
        if (bodies.Count == 0)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                RigidBody2D rigidBody;
                StaticBody2D staticBody;
                string errorMessage;

                if (i < 3)
                {
                    // Create bodies using positions and colors
                    PhysicsBody2D.CreateRigidBody(positions[i], 0f, Vector2.One, 1, 0.5f, ShapeType.Circle, 32f, 0f, 0f, out rigidBody, out errorMessage);

                    // Add bodies to the list
                    bodies.Add(rigidBody);
                }

                else
                {
                    PhysicsBody2D.CreateStaticBody(positions[i], 0f, Vector2.One, 0.5f, ShapeType.Box, 0f, 64f, 960f, out staticBody, out errorMessage);
                    bodies.Add(staticBody);
                }

            }
        }


        // Update and draw each body
        for (int i = 0; i < bodies.Count; i++)
        {
            RenderBody2D.RenderPhysicsObject(bodies[i], colors[i]);
        }
    }
}
