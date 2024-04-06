using PhysicsEngine.src.physics._2D.body;
using System.Numerics;

namespace PhysicsEngine.src.world;


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
        System.Console.WriteLine(boxes.Count);
        // Iterate through the edges and create the bodies
        foreach (Box box in boxes)
        {
            System.Console.WriteLine(box);
            Vector2 position = size * (new Vector2((box.left + box.right + 1) / 2.0f,(box.y + 1 ) / 2));
            float width = ((box.right - box.left) + 1) * size;
            BodyCreation.CreateStaticBody(position, 0f, new Vector2(1, 1), 0.5f, width, size, out StaticBody2D staticBody);
            bodies.Add(staticBody);
        }
    }
}
