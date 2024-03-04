using PhysicsEngine.src.physics._2D;
using System.Numerics;

namespace PhysicsEngine.src.body;

public sealed class StaticBody2D : PhysicsBody2D
{
    // Constructor
    public StaticBody2D(Vector2 position, float rotation, Vector2 scale, 
        float mass, float restitution, float area, float radius, float width, float height, ShapeType shape) 
    {
        Transform = new Transform2D(position, rotation, scale);
        Dimensions = new Dimensions2D(radius, width, height);

        // Denisty = Mass, since Mass = Infinity
        Substance = new Substance2D(mass, mass, area, restitution, true);

        this.LinVelocity = Vector2.Zero;
        this.RotVelocity = 0f;

        Shape = shape;

        // Map vertices to rigid body based on the shape
        MapVertices(shape);

        verticesUpdateRequired = true;
    }
}


