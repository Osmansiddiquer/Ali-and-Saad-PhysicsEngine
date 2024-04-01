using PhysicsEngine.src.body;
using PhysicsEngine.src.components;
using System.Numerics;

namespace PhysicsEngine.src.physics._2D.body;

public class RigidCircle2D : RigidBody2D
{
    // Constructor
    public RigidCircle2D(Vector2 position, float rotation, Vector2 scale, float mass, float density, float area, float restitution,
        float radius, List<Component> components) : base(position, rotation, scale, mass, density, area, restitution, ShapeType.Circle, components)
    {
        Dimensions = new Dimensions2D(radius);
        MapVerticesCircle();
    }
}
