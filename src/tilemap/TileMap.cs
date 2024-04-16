using GameEngine.src.physics.body;
using System.Numerics;
using Raylib_cs;
using GameEngine.src.world;

namespace GameEngine.src.tilemap;


public class Box
{
    public int left;
    public int right;
    public int y;

    public override string ToString()
    {
        return $"Left: {left}, Right: {right}, Y: {y}";
    }
}

internal static class TileMap
{
    public static void GenerateTileMap(int[,] grid, int size, List<PhysicsBody2D> bodies)
    {
        // Find all the boxes in the grid
        List<Box> boxes = FindBoxes(grid);

        // Create the bodies
        AddBodies(boxes, bodies, (int)Math.Pow(2, size + 2));
    }
    public static List<Box> FindBoxes(int[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        List<Box> boxes = new List<Box>();

        for (int i = 0; i < rows; i++)
        {
            int start, end;
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j] == 1)
                {
                    start = j;
                    while (j < cols && grid[i, j] == 1)
                    {
                        j++;
                    }
                    end = j - 1;
                    boxes.Add(new Box() { left = start, right = end, y = i });
                }
            }
        }

        return boxes;
    }


    public static void AddBodies(List<Box> boxes, List<PhysicsBody2D> bodies, int size)
    {
        Console.WriteLine(boxes.Count);
        // Iterate through the edges and create the bodies
        foreach (Box box in boxes)
        {
            Console.WriteLine(box);
            Vector2 position = size * new Vector2((box.left + box.right + 1) / 2.0f, (box.y + 1) / 2);
            float width = (box.right - box.left + 1) * size;
            WorldCreation.CreateStaticBody(position, 0f, new Vector2(1, 1), 0.5f, width, size, out StaticBody2D staticBody);
            bodies.Add(staticBody);
        }
    }

    public static Texture2D[,] GenerateBackground(int[,] textureMap)
    {
        Texture2D[,] backGround = new Texture2D[textureMap.GetLength(0), textureMap.GetLength(1)];
        for (int i = 0; i < textureMap.GetLength(0); i++)
        {
            for (int j = 0; j < textureMap.GetLength(1); j++)
            {
                //try
                //{
                //    backGround[i, j] = Raylib.LoadTexture($"C:/Users/saadk/Desktop/NUST/Semester 2/Object Oriented Programming/End Semester Project/Physics Engine/res/scenes/background{textureMap[i, j]}.png");
                //} catch (Exception e)
                //{
                //    Console.WriteLine("hello!");
                //    backGround[i, j] = Raylib.LoadTexture($"C:/Users/saadk/Desktop/NUST/Semester 2/Object Oriented Programming/End Semester Project/Physics Engine/res/scenes/Terrain (16x16).png");
                //}

                if (textureMap[i, j] != 0)
                {
                    backGround[i, j] = Raylib.LoadTexture("C:/Users/saadk/Desktop/NUST/Semester 2/Object Oriented Programming/End Semester Project/Physics Engine/res/scenes/background1.png");
                }
                else
                {
                    backGround[i, j] = Raylib.LoadTexture("C:/Users/saadk/Desktop/NUST/Semester 2/Object Oriented Programming/End Semester Project/Physics Engine/res/scenes/Terrain (16x16).png");
                }
            }
        }

        return backGround;
    }

    public static void DrawBackground(Texture2D[,] backGround)
    {
        for (int i = 0; i < backGround.GetLength(0); i++)
        {
            for (int j = 0; j < backGround.GetLength(1); j++)
            {
                Rectangle sourceRec = new Rectangle(0, 0, 48, 48);
                Rectangle destRec = new Rectangle(i * 60, j * 60, 60, 60);
                // Raylib.DrawTextureRec(backGround[i, j], sourceRec, new Vector2(i * 60, j * 60), Color.White);

                Raylib.DrawTexturePro(backGround[i, j], sourceRec, destRec, new Vector2(0, 0), 0, Color.White);
            }
        }
    }
}
