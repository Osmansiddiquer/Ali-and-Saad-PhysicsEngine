using GameEngine.src.main;
using GameEngine.src.world;
using Raylib_cs;
using System.Numerics;
using GameEngine.src.physics.body;

namespace GameEngine.res.scenes;

public class Scene : World2D
{
    // Member variables
    private static List<PhysicsBody2D> bodies;
    private static List<Color> colors;
    //public static TileMapProps tileMapProps;

    // Constructor for initialization
    static Scene()
    {
        colors = new List<Color>() { 
            Color.White, 
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Gold
        };

        bodies = new List<PhysicsBody2D>();

        //tileMapProps = new TileMapProps()
        //{
        //    tileMap = new int[,]
        //        {
        //            {1, 1, 1, 1, 1, 1, 1, 0, 1, 0},
        //            {0, 1, 0, 1, 0, 1, 0, 1, 0, 1},
        //            {1, 0, 1, 0, 1, 0, 1, 0, 1, 0},
        //            {0, 1, 0, 1, 0, 1, 0, 1, 0, 1},
        //            {1, 0, 1, 0, 1, 0, 1, 0, 1, 0},
        //            {0, 1, 0, 1, 0, 1, 0, 1, 0, 1},
        //            {1, 0, 1, 0, 1, 0, 1, 0, 1, 0},
        //            {0, 1, 0, 1, 0, 1, 0, 1, 0, 1},
        //            {1, 0, 1, 0, 1, 0, 1, 0, 1, 0},
        //            {0, 1, 0, 1, 0, 1, 0, 1, 0, 1}
        //        },
        //    textureMap = new int[,]
        //        {
        //            {1, 1, 1},
        //            {1, 1, 1},
        //            {1, 0, 2}
        //        },
        //    size = 4,
        //};

        //TileMap.GenerateTileMap(ref tileMapProps, bodies);
    }

    // Ready function (Runs on first frame)
    public static void Ready()
    {
        Properties.DisplayFPS = true;
        
    }

    public static void Update(double delta)
    {
        //// Create a camera centered at the middle of the screen
        //Camera2D camera = new Camera2D(Vector2.Zero, Vector2.Zero, 0, 1f);

        //if (bodies.Count > 1)
        //{
        //    camera.Target = bodies[1].Transform.Translation;
        //    camera.Offset = new Vector2(640, 480);
        //}

        // Begin 2D mode with the camera
        //Raylib.BeginMode2D(camera);

        // Draw
        Draw();

        // End 2D mode
        //Raylib.EndMode2D();

        // Handle physics outside the 2D mode
        HandlePhysics(bodies, delta);
    }

    // Draw
    public static void Draw()
    {
        // Ensure bodies are created (call once or in Ready)
        if (bodies.Count == 0) { 
            CreateStaticBody(new Vector2(640, 900), 0f, new Vector2(0.9f, 0.9f), 0.5f, 1280f, 120f, out StaticBody2D staticBody);
            bodies.Add(staticBody);

        }

        if (Raylib.IsMouseButtonPressed(MouseButton.Left)) {
            
            // Create circle rigid body
            CreateRigidBody(Raylib.GetMousePosition(), Vector2.One, 1f, 0.5f, 32f, out RigidBody2D rigidBody);
            bodies.Add(rigidBody);

            // bodies.Add(new RigidCircle2D(Raylib.GetMousePosition(), Vector2.One, 1f, 0.5f, 32f));
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

        //TileMap.DrawBackground(tileMapProps);

        // Update and draw each body
        for (int i = 0; i < bodies.Count; i++) {
            RenderPhysicsObject(bodies[i], colors[i % 5]);
        }

    }
}
