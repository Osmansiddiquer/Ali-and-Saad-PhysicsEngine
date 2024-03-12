using PhysicsEngine.src.body;
using PhysicsEngine.src.main;
using PhysicsEngine.src.physics;
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
        Collisions.HandleCollision(bodies);

        foreach (PhysicsBody2D body in bodies)
        {
            body.RunComponents();
        }
    }

    // Draw
    public static void Draw()
    {
        // Ensure bodies are created (call once or in Ready)
        if (bodies.Count == 0) { 
            CreateStaticBody(new Vector2(480, 600), 0f, Vector2.One, 
                0.5f, 64f, 960f, out StaticBody2D staticBody, out string errorMessage);
            
            bodies.Add(staticBody);  
        }

        if (Raylib.IsMouseButtonPressed(MouseButton.Left)) {
            CreateRigidBody(Raylib.GetMousePosition(), 0f, Vector2.One,
                1f, 0.5f, 32f, out RigidBody2D rigidBody, out string errorMessage);

            bodies.Add(rigidBody);
        }


        // Update and draw each body
        for (int i = 0; i < bodies.Count; i++) {
            RenderPhysicsObject(bodies[i], colors[i % 4]);
        }
    }
}
