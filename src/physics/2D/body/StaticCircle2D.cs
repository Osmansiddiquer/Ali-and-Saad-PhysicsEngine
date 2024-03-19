using PhysicsEngine.src.body;
using PhysicsEngine.src.components;
using System.Numerics;

namespace PhysicsEngine.src.physics._2D.body;

public class StaticCircle2D : StaticBody2D
{
    // Constructor 
    public StaticCircle2D(Vector2 position, float rotation, Vector2 scale, 
        float mass, float area, float restitution, float radius) : base(position, rotation, scale, mass, restitution, area, ShapeType.Circle)
    {
        Dimensions = new Dimensions2D(radius, 0, 0);
        MapVerticesCircle();
    }
}
