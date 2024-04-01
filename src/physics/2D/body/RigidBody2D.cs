using PhysicsEngine.src.physics._2D;
using System.Numerics;
using PhysicsEngine.src.components;
using System.Transactions;

namespace PhysicsEngine.src.body;

public class RigidBody2D : PhysicsBody2D
{
    // Force applied to the body
    public Vector2 Force;

    private List<Component> components = new List<Component>();


    // Constructor
    public RigidBody2D(Vector2 position, float rotation, Vector2 scale, float mass, float density, float area, 
        float restitution, ShapeType shape, List<Component> components) : base (position, rotation, scale)
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

    public void Rotate(float angle)
    {
        Transform.Rotate(angle);
        VerticesUpdateRequired = true;
        AABBUpdateRequired = true;
    }

    public override void ApplyForce(Vector2 amount)
    {
        Force = amount;
    }

    // Run the list of components
    public override void RunComponents()
    {
        foreach(Component component in components)
        {
            component.RunComponent(this);
        }
    }
}

