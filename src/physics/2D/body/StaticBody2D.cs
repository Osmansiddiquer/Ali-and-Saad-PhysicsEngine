using PhysicsEngine.src.physics._2D;
using System.Numerics;

namespace PhysicsEngine.src.body;

public class StaticBody2D : PhysicsBody2D
{
    // Constructor
    public StaticBody2D(Vector2 position, float rotation, Vector2 scale,
        float mass, float restitution, float area, ShapeType shape) : base(position, rotation, scale)
    {
        // Density = Mass, since Mass = Infinity
        Substance = new Substance2D(mass, mass, area, restitution, true);

        LinVelocity = Vector2.Zero;
        RotVelocity = 0f;

        Shape = shape;

        verticesUpdateRequired = true;
    }
}


