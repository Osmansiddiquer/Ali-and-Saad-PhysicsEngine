using GameEngine.src.world;
using Raylib_cs;
using System.Numerics;
using GameEngine.src.physics.body;
using GameEngine.src.input;
using GameEngine.src.helper;

namespace GameEngine.res.scenes;

public class CollisionTest : World2D
{
    // Member variables
    private List<PhysicsBody2D> bodies;
    private List<Color> colors;


    // Constructor for initialization
    internal CollisionTest()
    {
        colors = new List<Color>() { 
            Color.White, 
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Gold
        };

        bodies = new List<PhysicsBody2D>();

        Raylib.HideCursor();
    }

    public override void Update(double delta)
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

        float rotation = Input.GetDirection("left", "right") / 5;

        // Draw
        Draw();

        bodies[0].Rotate(rotation);

        // End 2D mode
        //Raylib.EndMode2D();

        // Handle physics outside the 2D mode
        HandlePhysics(bodies, delta);
    }

    // Draw
    private void Draw()
    {
        Raylib.DrawText("Collision Test", 20, 20, 32, Color.Green);

        // Ensure bodies are created (call once or in Ready)
        if (bodies.Count == 0) { 
            CreateStaticBody(new Vector2(640, 900), 0f, Vector2.One, 0.5f, 1200f, 100f, out StaticBody2D staticBody);
            bodies.Add(staticBody);

        }

        // Random
        Random random = new Random();
        float x = (float)(random.NextDouble() * (1.2 - 1) + 1);
        float y = (float)(random.NextDouble() * (1.2 - 1) + 1);

        Vector2 scale = new Vector2((float)x, (float)y);

        if (Mouse.IsRMBPressed()) {
            
            // Create circle rigid body
            CreateRigidBody(Mouse.GetPos(), scale, 1f, 0.5f, 32f, out RigidBody2D rigidBody);
            bodies.Add(rigidBody);


            // bodies.Add(new RigidCircle2D(Raylib.GetMousePosition(), Vector2.One, 1f, 0.5f, 32f));
        }

        else if (Mouse.IsLMBPressed()) {

            // Create box rigid body
            CreateRigidBody(Raylib.GetMousePosition(), 0f, scale, 1f, 0.5f, 64f, 64f, out RigidBody2D rigidBody);
            bodies.Add(rigidBody);

        } 

        //TileMap.DrawBackground(tileMapProps);

        // Update and draw each body
        for (int i = 0; i < bodies.Count; i++) {
            RenderCollisionShapes(bodies[i], colors[i % 5]);
        }
        Raylib.DrawText("<>", Mouse.GetX(), Mouse.GetY(), 32, Color.Green);
    }
}
