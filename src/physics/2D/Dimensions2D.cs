using System.Numerics;

namespace PhysicsEngine.src.physics._2D;
public class Dimensions2D
{

    // Shape dimensions for a body
    public readonly float Radius;
    public readonly float Height;
    public readonly float Width;

    public Dimensions2D() { }
    public Dimensions2D(float radius)
    {
        Radius = radius;
    }

    public Dimensions2D(Vector2 size)
    {
        Width = size.X;
        Height = size.Y;
    }
}
