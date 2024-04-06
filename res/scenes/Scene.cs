using PhysicsEngine.src.physics._2D.body;
using PhysicsEngine.src.main;
using PhysicsEngine.src.world;
using Raylib_cs;
using System.Numerics;

namespace PhysicsEngine.res.scenes;

public class Scene : PhysicsWorld2D
{
    // Member variables
    private static List<PhysicsBody2D> bodies;
    private static List<Color> colors;


    // Constructor for initialization
    static Scene()
    {
        colors = new List<Color>() { 
            Color.White, 
            Color.Red,
            Color.Green,
            Color.Blue,
        };

        bodies = new List<PhysicsBody2D>();
       
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
        HandlePhysics(bodies, delta);

        if (bodies.Count > 1) { }
    }

    // Draw
    public static void Draw()
    {
        // Ensure bodies are created (call once or in Ready)
        if (bodies.Count == 0) { 
            CreateStaticBody(new Vector2(640, 900), 0f, new Vector2(0.9f, 0.9f), 0.5f, 1280f, 120f, out StaticBody2D staticBody);
            bodies.Add(staticBody);

            int[,] tileMap = new int[,]
            {
                {1, 1, 1, 1, 1},
                {1, 0, 0, 0, 1},
                {1, 0, 0, 0, 1},
                {1, 1, 1, 1, 1}
            };
            // Use tilemap
            TileMap.GenerateTileMap(tileMap, 4, bodies);
        }

        if (Raylib.IsMouseButtonPressed(MouseButton.Left)) {
            
            // Create circle rigid body
            CreateRigidBody(Raylib.GetMousePosition(), Vector2.One, 1f, 0.5f, 32f, out RigidBody2D rigidBody);
            bodies.Add(rigidBody);
            Console.WriteLine(bodies.Count);
        }

        else if (Raylib.IsMouseButtonPressed(MouseButton.Right)) {

            // Create box rigid body
            CreateRigidBody(Raylib.GetMousePosition(), 0f, Vector2.One, 1f, 0.5f, 64f, 64f, out RigidBody2D rigidBody);
            bodies.Add(rigidBody);

        } 

        else if(Raylib.IsKeyPressed(KeyboardKey.Space))
        {
            Random random = new Random();

            float magnitude = 3;

            Vector2 velocity = new Vector2(((float)random.NextDouble() * 2 - 1) * magnitude, ((float)random.NextDouble() * 2 - 1) * magnitude);

            // Create projectile
            CreateProjectileBody(Raylib.GetMousePosition(), Vector2.One, 1f, 0.5f, 16f, velocity * 0.2f, bodies, out RigidBody2D rigidBody);
            bodies.Add(rigidBody);
        }

        // Update and draw each body
        for (int i = 0; i < bodies.Count; i++) {
            RenderPhysicsObject(bodies[i], colors[i % 4]);
        }
    }
}
