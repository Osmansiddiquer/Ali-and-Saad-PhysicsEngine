using PhysicsEngine.src.body;
using System.Numerics;

namespace PhysicsEngine.src.physics._2D.body;

public class StaticBox2D : StaticBody2D
{
    // Constructor
    public StaticBox2D(Vector2 position, float rotation, Vector2 scale,
        float mass, float area, float restitution, float width, float height) : base(position, rotation, scale, mass, restitution, area, ShapeType.Box)
    {
        Dimensions = new Dimensions2D(0, width, height);
        MapVerticesBox();
    }
}