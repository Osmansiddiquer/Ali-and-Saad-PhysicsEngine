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
    public static Texture2D texture;
    public static Texture2D[,] backGround;

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
        texture = Raylib.LoadTexture("C:/Users/saadk/Desktop/NUST/Semester 2/Object Oriented Programming/End Semester Project/Physics Engine/res/scenes/background3.png");
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
                {0, 0, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0}
            };
            // Use tilemap
            TileMap.GenerateTileMap(tileMap, 4, bodies);

            int[,] backGroundArray = new int[,]
            {
                {1, 0, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 1}
            };

            backGround = TileMap.GenerateBackground(backGroundArray);
        }

        if (Raylib.IsMouseButtonDown(MouseButton.Left)) {
            
            // Create circle rigid body
            CreateRigidBody(Raylib.GetMousePosition(), Vector2.One, 1f, 0.5f, 12f, out RigidBody2D rigidBody);
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

        TileMap.DrawBackground(backGround);
    }
}
