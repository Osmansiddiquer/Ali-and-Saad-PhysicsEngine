﻿using GameEngine.src.physics.body;
using GameEngine.src.world;
using Raylib_cs;
using System.Numerics;
using GameEngine.src.tilemap;

namespace GameEngine.res.scenes;

internal class PlayerTest : World2D
    {
    private TileMapProps tileMapProps;
    private List<PhysicsBody2D> bodies;
    private List<Color> colors;

    internal PlayerTest()
    {
        bodies = new List<PhysicsBody2D>();

        // Create player
        CreatePlayerBody(new Vector2(128, 512), 0, 1f, 64f, 128f, out PlayerBody2D player);
        bodies.Add(player);

        colors = new List<Color>() {
            Color.White,
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Gold
        };

        // Tilemap
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

        Raylib.ShowCursor();

    }

    public override void Update(double delta)
    {            
        Draw();
        HandlePhysics(bodies, delta);

        PlayerBody2D player = (PlayerBody2D)bodies[0];
        player.DrawPlayer();
        player.UseDefaultMotion(delta);
    }

    private void Draw()
    {
        // Scene title
        Raylib.DrawText("Player Test", 20, 20, 32, Color.Green);

        for (int i = 0; i < bodies.Count; i++)
        {
            DrawCollisionShapes(bodies[i], colors[i % 5]);
        }
    }

}

