using PhysicsEngine.src.physics._2D.body;
using PhysicsEngine.src.physics._2D.collision;
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
        HandlePhysics(bodies);

        if (bodies.Count > 2 ) { 
            //Console.WriteLine(bodies[2].LinVelocity);
        }
    }

    // Draw
    public static void Draw()
    {
        // Ensure bodies are created (call once or in Ready)
        if (bodies.Count == 0) { 
            CreateStaticBody(new Vector2(640, 900), 0f, new Vector2(0.9f, 0.9f), 0.5f, 1280f, 120f, out StaticBody2D staticBody);

            staticBody.Name = "Ground";
            staticBody.Substance.StaticFriction = 0.5f;
            staticBody.Substance.DynamicFriction = 0.5f;

            bodies.Add(staticBody);  
        }

        if (Raylib.IsMouseButtonPressed(MouseButton.Left)) {
            
            // Create circle rigid body
            CreateRigidBody(Raylib.GetMousePosition(), Vector2.One, 1f, 0.5f, 32f, out RigidBody2D rigidBody);

            rigidBody.Name = ("Circle " + bodies.Count);
            rigidBody.Substance.StaticFriction = 0.5f;
            rigidBody.Substance.DynamicFriction = 0.5f;

            bodies.Add(rigidBody);
        }

        else if(Raylib.IsMouseButtonPressed(MouseButton.Right)){

            // Create box rigid body
            CreateRigidBody(Raylib.GetMousePosition(), 0f, Vector2.One, 1f, 0.5f, 64f, 64f, out RigidBody2D rigidBody);    
            bodies.Add(rigidBody);
        } else if(Raylib.IsKeyPressed(KeyboardKey.Space)) {
            int magnitude = 10;

            Random rand = new Random();

            // get a random direction which is a unit vector
            Vector2 direction = new Vector2((float)rand.NextDouble(), (float)rand.NextDouble());
            direction = Vector2.Normalize(direction);

            Vector2 velocity = magnitude * direction;


            // create a projectile body if you press spacebar
            CreateProjectileBody(Raylib.GetMousePosition(), Vector2.One, 1f, 0.5f, 32f, velocity, bodies, out RigidBody2D rigidBody);

            bodies.Add(rigidBody);
        }


        // Update and draw each body
        for (int i = 0; i < bodies.Count; i++) {
            RenderPhysicsObject(bodies[i], colors[i % 4]);
        }
    }
}
