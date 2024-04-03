using System.Numerics;

namespace PhysicsEngine.src.physics._2D.collision;

// Create a box boundary around a shape
public class AxisAlignedBoundingBox
{
    // Lower and upper side of box
    public Vector2 Min { get; private set; }
    public Vector2 Max { get; private set; }

    public AxisAlignedBoundingBox() { }

    // Constructors
    public AxisAlignedBoundingBox(Vector2 min, Vector2 max)
    {
        Min = min;
        Max = max;
    }
    
    public AxisAlignedBoundingBox(float minX, float minY, float maxX, float maxY) 
    {
        Min = new Vector2(minX, minY);
        Max = new Vector2(maxX, maxY);
    }

    
}