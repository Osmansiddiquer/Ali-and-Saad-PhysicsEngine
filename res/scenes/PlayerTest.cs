using GameEngine.src.physics.body;
using GameEngine.src.world;
using Raylib_cs;
using System.Numerics;
using GameEngine.src.tilemap;
using GameEngine.src.input;
using static System.Formats.Asn1.AsnWriter;

namespace GameEngine.res.scenes;

internal class PlayerTest : World2D
    {
    private TileMapProps tileMapProps;
    private List<PhysicsBody2D> bodies;
    private List<Color> colors;

    internal PlayerTest()
    {
        bodies = new List<PhysicsBody2D>();

        colors = new List<Color>() {
            Color.White,
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Gold
        };


        tileMapProps = new TileMapProps()
        {
            tileMap = new int[,]
                {
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                    {0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0},
                    {0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0},
                    {0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0},
                    {0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0, 1, 1, 1, 0}

                },
            textureMap = new int[,]
                {
                    {1, 1, 1},
                    {1, 1, 1},
                    {1, 0, 2}
                },
            size = 4,
        };

        TileMap.GenerateTileMap(ref tileMapProps, bodies);

        // Create player
        WorldCreation.CreatePlayerBody(new Vector2(100, 100), 0, Vector2.One, 1f, 64f, 128f, out RigidBody2D player);
        bodies.Add(player);

        Raylib.ShowCursor();

    }

    public override void Update(double delta)
    {
        Raylib.DrawText("Player Test", 20, 20, 32, Color.Green);
        Draw();
        HandlePhysics(bodies, delta);
    }

    private void Draw()
    {
        if (InputMap.IsLMBPressed())
        {
            // Create box rigid body
            CreateRigidBody(Raylib.GetMousePosition(), 0f, Vector2.One, 1f, 0.5f, 64f, 64f, out RigidBody2D rigidBody);
            bodies.Add(rigidBody);
        }

        for (int i = 0; i < bodies.Count; i++)
        {
            RenderPhysicsObject(bodies[i], colors[i % 5]);
            if (bodies[i] is PlayerBody2D)
            {
                PlayerBody2D player = (PlayerBody2D)bodies[i];
                player.DrawPlayer();
            }
        }
    }

}

