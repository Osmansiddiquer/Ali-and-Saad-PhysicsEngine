using PhysicsEngine.src.body;
using PhysicsEngine.src.components;
using System.Numerics;

namespace PhysicsEngine.src.physics._2D.body;

public class RigidBox2D : RigidBody2D
{

    // Constructor
    public RigidBox2D(Vector2 position, float rotation, Vector2 scale, float mass, float density, float area, float restitution, 
        float width, float height, List<Component> components) : base (position, rotation, scale, mass, density, 
            area, restitution, ShapeType.Box, components)
    {
        Dimensions = new Dimensions2D(new Vector2(width, height) * scale);
        MapVerticesBox();
    }
}
