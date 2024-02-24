namespace PhysicsEngine.src.physics._2D;
public class Dimensions2D
{

    // Shape dimensions for a body
    public float Radius { get; private set; }
    public float Height { get; private set; }
    public float Width  { get; private set; }

    public Dimensions2D(float radius, float height, float width)
    {
        Radius = radius;
        Height = height;
        Width = width;
    }
}
