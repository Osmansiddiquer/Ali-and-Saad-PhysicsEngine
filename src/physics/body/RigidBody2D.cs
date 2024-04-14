using System.Numerics;
using GameEngine.src.physics.component;

namespace GameEngine.src.physics.body;

public class RigidBody2D : PhysicsBody2D
{
    // Force applied to the body
    public Vector2 Force { get; internal set; }
    protected List<Component> components = new List<Component>();

    // Constructor
    internal RigidBody2D(Vector2 position, float rotation, Vector2 scale, float mass, float density, float area,
        float restitution, ShapeType shape, List<Component> components) : base(position, rotation, scale)
    {
        // Create the material for the body
        Material = new Material2D(mass, density, area, restitution);
        Shape = shape;

        // Initialize velocity and force
        LinVelocity = Vector2.Zero;
        RotVelocity = 0f;
        Force = Vector2.Zero;

        // Get components list 
        this.components = components;
    }

    // Self explanatory 
    public override void Translate(Vector2 direction)
    {
        Transform.Translate(direction);
        VerticesUpdateRequired = true;
        AABBUpdateRequired = true;
    }

    public override void Rotate(float angle)
    {
        Transform.Rotate(angle);
        VerticesUpdateRequired = true;
        AABBUpdateRequired = true;
    }

    public override void Scale(Vector2 factor)
    {
        Transform.Scaling(factor);
        VerticesUpdateRequired = true;
        AABBUpdateRequired = true;
    }

    public override void ApplyForce(Vector2 amount)
    {
        Force = amount;
    }

    // Add or remove components 
    public void AddComponent(Component component) { components.Add(component); }
    public void DelComponent(Component component) 
    {
        if (components.Contains(component))
            components.Remove(component);

        else Console.WriteLine("[WARN]: Components list does not contain " + component.ToString());
    }

    // Run the list of components
    internal override void RunComponents(double delta)
    {
        foreach (Component component in components)
        {
            component.RunComponent(this, delta);
        }
    }
}

public class RigidBox2D : RigidBody2D
{
    // Constructor
    internal RigidBox2D(Vector2 position, float rotation, Vector2 scale, float mass, float density, float area, float restitution,
        float width, float height, List<Component> components) : base(position, rotation, scale, mass, density,
            area, restitution, ShapeType.Box, components)
    {
        // Initialize dimensions and vertices
        Dimensions = new Dimensions2D(new Vector2(width, height) * scale);
        MapVerticesBox();

        // I = m/12 * (w^2 + h^2)
        MomentOfInertia = (1f / 12) * mass * (width * width + height * height);

    }
}

public class RigidCircle2D : RigidBody2D
{
    // Constructor
    internal RigidCircle2D(Vector2 position, Vector2 scale, float mass, float density, float area, float restitution,
        float radius, List<Component> components) : base(position, 0f, scale, mass, density, area, restitution, ShapeType.Circle, components)
    {
        // Initialize dimensions
        Dimensions = new Dimensions2D(radius * Vector2.Distance(Vector2.Zero, scale));

        // I = m/2 * r^2
        MomentOfInertia = (1f / 2) * mass * (radius * radius);
    }
}