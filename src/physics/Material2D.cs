
namespace GameEngine.src.physics;
public class Material2D
{
    // Physicsal attributes for a rigidbody object
    public readonly float Mass;
    public readonly float Density;
    public readonly float Area;
    public readonly float Restitution;

    public readonly float StaticFriction;
    public readonly float DynamicFriction;

    // Constructor
    public Material2D() { }
    public Material2D(float mass, float density, float area, float restitution)
    {
        Mass = mass;
        Density = density;
        Area = area;
        Restitution = restitution;

        // Default friction values (testing)
        StaticFriction = 0.5f;
        DynamicFriction = 0.3f;
    }
}
