using System.Numerics;

namespace PhysicsEngine.src.physics._2D.collision;

public class AxisAlignedBoundingBox
{
    public Vector2 Min { get; private set; }
    public Vector2 Max { get; private set; }

    public AxisAlignedBoundingBox() { }
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