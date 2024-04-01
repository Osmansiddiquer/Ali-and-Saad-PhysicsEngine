namespace PhysicsEngine.src.physics._2D;
public class Dimensions2D
{

    // Shape dimensions for a body
    public float Radius { get; private set; }
    public float Height { get; private set; }
    public float Width  { get; private set; }

    public Dimensions2D(float radius)
    {
        Radius = radius;
    }

    public Dimensions2D(float width, float height)
    {
        Height = height;
        Width = width;
    }
}
