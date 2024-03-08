using PhysicsEngine.src.physics._2D;
using System.Numerics;
using PhysicsEngine.src.components;

namespace PhysicsEngine.src.body;

public class RigidBody2D : PhysicsBody2D
{
    // Force applied to the body
    public Vector2 Force;

    public readonly int[]? Triangles;

    private List<Component> components = new List<Component>();


    // Constructor
    public RigidBody2D(Vector2 position, float rotation, Vector2 scale, float mass, float density, float area,
        float restitution, float radius, float width, float height, ShapeType shape, List<Component> components)
    {
        Transform = new Transform2D(position, rotation, scale);
        Dimensions = new Dimensions2D(radius, width, height);
        Substance = new Substance2D(mass, density, area, restitution, false);

        Shape = shape;

        LinVelocity = Vector2.Zero;
        RotVelocity = 0f;

        Force = Vector2.Zero;

        this.components = components;

        // Create vertices for box shape
        if (shape is ShapeType.Box) {
            vertices = CreateVerticesBox(Dimensions.Width, Dimensions.Height);
            transformedVertices = new Vector2[vertices.Length];

            Triangles = CreateTrianglesBox();
        }

        // No vertices for circle
        else {
            vertices = null;
            transformedVertices = null;

            Triangles = null;
        }

        verticesUpdateRequired = true;
    }

    // Move the rigid body (self explanatory)
    public void Translate(Vector2 amount)
    {
        Transform.Translate(amount);
        verticesUpdateRequired = true;
    }

    public void Rotate(float angle)
    {
        Transform.Rotate(angle);
        verticesUpdateRequired = true;
    }

    public void ApplyForce(Vector2 amount)
    {
        Force = amount;
    }

    public void RunComponents()
    {
        foreach(Component component in components)
        {
            component.RunComponent(this);
        }
    }
}

