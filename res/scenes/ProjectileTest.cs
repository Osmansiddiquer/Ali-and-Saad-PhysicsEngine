using GameEngine.src.input;
using GameEngine.src.main;
using GameEngine.src.physics.body;
using GameEngine.src.world;
using Raylib_cs;
using System.Numerics;

namespace GameEngine.res.scenes;

public class ProjectileTest : World2D
{
    private static List<PhysicsBody2D> bodies;
    private static List<Color> colors;
    private static Vector2 spawnPosition;

    static ProjectileTest()
    {
        colors = new List<Color>() {
            Color.White,
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Gold
        };

        bodies = new List<PhysicsBody2D>();
        spawnPosition = new Vector2(Properties.ScreenWidth / 2, Properties.ScreenHeight / 2);
    }

    public static void Update(double delta)
    {
        Raylib.DrawText("Projectile Test", 20, 20, 32, Color.Green);

        Draw();
        HandlePhysics(bodies, delta);
    }

    private static void Draw()
    {
        Raylib.DrawText("<>", Raylib.GetMouseX(), Raylib.GetMouseY(), 32, Color.Green);
        Raylib.HideCursor();

        if (InputMap.IsLMBPressed())
        {
            Vector2 velocity = (Raylib.GetMousePosition() - spawnPosition);
            velocity /= 128;

            // Create projectile
            CreateProjectileBody(spawnPosition, Vector2.One, 1f, 0.5f, 16f, velocity * 0.2f, bodies, out RigidBody2D body);
            bodies.Add(body);

        }

        // Update and draw each body
        for (int i = 0; i < bodies.Count; i++)
        {
            RenderPhysicsObject(bodies[i], colors[i % 5]);
        }
    }



}