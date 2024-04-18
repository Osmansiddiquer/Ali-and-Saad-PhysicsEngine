using System.Numerics;

namespace GameEngine.src.physics.body;

public class StaticBody2D : PhysicsBody2D
{
    // Constructor
    internal StaticBody2D(Vector2 position, float rotation, Vector2 scale, float restitution, ShapeType shape) 
        : base(position, rotation, scale)
    {
        // Keep restitution in valid range
        restitution = Math.Clamp(restitution, 0.0f, 1.0f);

        // Create the material for the body
        // Density = Mass, since Mass = Infinity
        Material = new Material2D(float.PositiveInfinity, float.PositiveInfinity, restitution);
        Shape = shape;

        // Moment of Inertia = Infinity (for static bodies)
        MomentOfInertia = float.PositiveInfinity;
    }
}

public class StaticBox2D : StaticBody2D
{
    // Constructor
    public StaticBox2D(Vector2 position, float rotation, Vector2 scale, float area, float restitution, float width, float height) 
        : base(position, rotation, scale, restitution, ShapeType.Box)
    {
        // Initialize dimensions and vertices
        Dimensions = new Dimensions2D(new Vector2(width, height) * scale, area);
        MapVerticesBox();
    }
}

public class StaticCircle2D : StaticBody2D
{
    // Constructor 
    public StaticCircle2D(Vector2 position, Vector2 scale, float area, float restitution, float radius) 
        : base(position, 0f, scale, restitution, ShapeType.Circle)
    {
        // Initialize dimensions 
        Dimensions = new Dimensions2D(radius * scale.Length(), area);
    }
}
