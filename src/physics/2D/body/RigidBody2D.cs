using System.Numerics;
using PhysicsEngine.src.components;

namespace PhysicsEngine.src.physics._2D.body;

public class RigidBody2D : PhysicsBody2D
{
    // Force applied to the body
    public Vector2 Force;

    protected List<Component> components = new List<Component>();

    // Constructor
    public RigidBody2D(Vector2 position, float rotation, Vector2 scale, float mass, float density, float area,
        float restitution, ShapeType shape, List<Component> components) : base(position, rotation, scale)
    {
        Substance = new Substance2D(mass, density, area, restitution, false);

        Shape = shape;

        LinVelocity = Vector2.Zero;
        RotVelocity = 0f;

        Force = Vector2.Zero;

        this.components = components;

        VerticesUpdateRequired = true;
        AABBUpdateRequired = true;
    }

    // Self explanatory 
    public override void Translate(Vector2 amount)
    {
        Transform.Translate(amount);
        VerticesUpdateRequired = true;
        AABBUpdateRequired = true;
    }

    public override void Rotate(float angle)
    {
        Transform.Rotate(angle);
        VerticesUpdateRequired = true;
        AABBUpdateRequired = true;
    }

    public override void Scale(Vector2 amount)
    {
        Transform.Scaling(amount);
    }

    public override void ApplyForce(Vector2 amount)
    {
        Force = amount;
    }

    // Run the list of components
    public override void RunComponents()
    {
        foreach (Component component in components)
        {
            component.RunComponent(this);
        }
    }
}

public class RigidBox2D : RigidBody2D
{
    // Constructor
    public RigidBox2D(Vector2 position, float rotation, Vector2 scale, float mass, float density, float area, float restitution,
        float width, float height, List<Component> components) : base(position, rotation, scale, mass, density,
            area, restitution, ShapeType.Box, components)
    {
        Dimensions = new Dimensions2D(new Vector2(width, height) * scale);
        MapVerticesBox();
    }
}

public class RigidCircle2D : RigidBody2D
{
    // Constructor
    public RigidCircle2D(Vector2 position, Vector2 scale, float mass, float density, float area, float restitution,
        float radius, List<Component> components) : base(position, 0f, scale, mass, density, area, restitution, ShapeType.Circle, components)
    {
        Dimensions = new Dimensions2D(radius * Vector2.Distance(Vector2.Zero, scale));
        MapVerticesCircle();
    }
}