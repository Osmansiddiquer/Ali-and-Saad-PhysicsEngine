using GameEngine.src.main;
using GameEngine.src.world;
using Raylib_cs;
using System.Numerics;
using GameEngine.src.physics.body;
using GameEngine.src.input;

namespace GameEngine.res.scenes;

public class CollisionTest : World2D
{
    // Member variables
    private static List<PhysicsBody2D> bodies;
    private static List<Color> colors;


    // Constructor for initialization
    static CollisionTest()
    {
        colors = new List<Color>() { 
            Color.White, 
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Gold
        };

        bodies = new List<PhysicsBody2D>();

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


        Raylib.DrawText("Collision Test", 20, 20, 32, Color.Green);
        float rotation = InputMap.GetDirection("left", "right") / 5;

        // Draw
        Draw();

        bodies[0].Rotate(rotation);

        // End 2D mode
        //Raylib.EndMode2D();

        // Handle physics outside the 2D mode
        HandlePhysics(bodies, delta);
    }

    // Draw
    private static void Draw()
    {

        Raylib.HideCursor();

        // Ensure bodies are created (call once or in Ready)
        if (bodies.Count == 0) { 
            CreateStaticBody(new Vector2(640, 900), 0f, Vector2.One, 0.5f, 1200f, 100f, out StaticBody2D staticBody);
            bodies.Add(staticBody);

        }

        Random random = new Random();
        float scaling = (float)(random.NextDouble() * (1.1 - 0.9) + 0.9);

        Vector2 scale = new Vector2((float)scaling, (float)scaling);

        if (InputMap.IsRMBPressed()) {
            
            // Create circle rigid body
            CreateRigidBody(Raylib.GetMousePosition(), scale, 1f, 0.5f, 32f, out RigidBody2D rigidBody);
            bodies.Add(rigidBody);


            // bodies.Add(new RigidCircle2D(Raylib.GetMousePosition(), Vector2.One, 1f, 0.5f, 32f));
        }

        else if (InputMap.IsLMBPressed()) {

            // Create box rigid body
            CreateRigidBody(Raylib.GetMousePosition(), 0f, scale, 1f, 0.5f, 64f, 64f, out RigidBody2D rigidBody);
            bodies.Add(rigidBody);

        } 

        //TileMap.DrawBackground(tileMapProps);

        // Update and draw each body
        for (int i = 0; i < bodies.Count; i++) {
            RenderPhysicsObject(bodies[i], colors[i % 5]);
        }
        Raylib.DrawText("<>", Raylib.GetMouseX(), Raylib.GetMouseY(), 32, Color.Green);
    }
}
