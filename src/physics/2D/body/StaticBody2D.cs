using PhysicsEngine.src.physics._2D;
using System.Numerics;

namespace PhysicsEngine.src.physics._2D.body;

public class StaticBody2D : PhysicsBody2D
{
    // Constructor
    internal StaticBody2D(Vector2 position, float rotation, Vector2 scale,
        float mass, float restitution, float area, ShapeType shape) : base(position, rotation, scale)
    {
        // Density = Mass, since Mass = Infinity
        Substance = new Substance2D(mass, mass, area, restitution);
        Shape = shape;

        MomentOfInertia = 1f / 0;
    }
}

public class StaticBox2D : StaticBody2D
{
    // Constructor
    public StaticBox2D(Vector2 position, float rotation, Vector2 scale,
        float mass, float area, float restitution, float width, float height) : base(position, rotation, scale, mass, restitution, area, ShapeType.Box)
    {
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
        Dimensions = new Dimensions2D(radius * scale.Length());
    }
}
