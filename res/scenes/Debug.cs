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

    private static List<RigidBody2D> bodies;

    private static List<Vector2> positions;
    private static float rotation = 1f;
    private static Vector2 force = Vector2.Zero;


    // Constructor for initialization
    static Debug()
    {
        colors = new List<Color>() { Color.Red, Color.White };

        bodies = new List<RigidBody2D>();
        positions = new List<Vector2>() { new Vector2(64, 320), new Vector2(480, 320) };
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

        // Move circles based on input
        float dx = 0f;
        float dy = 0f;
        float magnitude = 30000f;

        if (Raylib.IsKeyDown(KeyboardKey.Left)) dx--;
        else if (Raylib.IsKeyDown(KeyboardKey.Right)) dx++;

        if (Raylib.IsKeyDown(KeyboardKey.Up)) dy--;
        else if (Raylib.IsKeyDown(KeyboardKey.Down)) dy++;

        if (Raylib.IsKeyDown(KeyboardKey.R)) bodies[0].Rotate(rotation);

        if (dx != 0 || dy != 0)
        {
            Vector2 direction = Vector2.Normalize(new Vector2(dx, dy)) * (float)delta;
            force = direction * magnitude;
            bodies[0].ApplyForce(force);
        }

        for (int i = 0; i < bodies.Count; i++) { bodies[i].Motion(); }

    }

    // Draw
    public static void Draw()
    {
        // Ensure bodies are created (call once or in Ready)
        if (bodies.Count == 0)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                PhysicsBody2D body;
                string errorMessage;

                // Create bodies using positions and colors
                PhysicsBody2D.CreateCircleBody(positions[i], Vector2.One, colors[i], 1f, 0.5f, 32, false, out body, out errorMessage); 

                // Add bodies to the list
                bodies.Add((RigidBody2D)body);
            }
        }

        // Update and draw each body
        for (int i = 0; i < bodies.Count; i++)
        {
            RenderBody2D.RenderPhysicsObject(bodies[i], colors[i]);
        }
    }
}
