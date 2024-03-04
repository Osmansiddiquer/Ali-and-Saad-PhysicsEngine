namespace PhysicsEngine.src.physics._2D;
public class Substance2D
{
    // Physicsal attributes for a rigidbody object
    public float Mass { get; private set; }
    public float Density { get; private set; }
    public float Area { get; private set; }
    public float Restitution { get; private set; }

    public bool IsStatic { get; private set; }

    // Constructor
    public Substance2D(float mass, float density, float area, float restitution, bool isStatic)
    {
        Mass = mass;
        Density = density;
        Area = area;
        Restitution = restitution;
        IsStatic = isStatic;
    }

}
