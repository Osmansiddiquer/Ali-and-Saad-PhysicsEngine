using System.Numerics;

namespace GameEngine.src.physics.body;

public class StaticBody2D : PhysicsBody2D
{
    // Constructor
    internal StaticBody2D(Vector2 position, float rotation, Vector2 scale,
        float mass, float restitution, float area, ShapeType shape) : base(position, rotation, scale)
    {
        // Create the material for the body
        // Density = Mass, since Mass = Infinity
        Material = new Material2D(mass, mass, area, restitution);
        Shape = shape;

        // Moment of Inertia = Infinity (for static bodies)
        MomentOfInertia = 1f / 0;
    }
}

public class StaticBox2D : StaticBody2D
{
    // Constructor
    public StaticBox2D(Vector2 position, float rotation, Vector2 scale,
        float mass, float area, float restitution, float width, float height) : base(position, rotation, scale, mass, restitution, area, ShapeType.Box)
    {
        // Initialize dimensions and vertices
        Dimensions = new Dimensions2D(new Vector2(width, height) * scale);
        MapVerticesBox();
    }
}

public class StaticCircle2D : StaticBody2D
{
    // Constructor 
    public StaticCircle2D(Vector2 position, Vector2 scale,
        float mass, float area, float restitution, float radius) : base(position, 0f, scale, mass, restitution, area, ShapeType.Circle)
    {
        // Initialize dimensions 
        Dimensions = new Dimensions2D(radius * scale.Length());
    }
}
