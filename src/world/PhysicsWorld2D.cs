using Raylib_cs;
using System.Numerics;
using PhysicsEngine.src.components;
using PhysicsEngine.src.physics._2D.body;
using PhysicsEngine.src.physics._2D.collision;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace PhysicsEngine.src.world;
public class PhysicsWorld2D
{
    // Constraints
    protected static readonly float MIN_BODY_SIZE = 0.01f * 0.01f;
    protected static readonly float MAX_BODY_SIZE = 2048f * 2048f;

    protected static readonly float MIN_DENSITY = 0.10f;
    protected static readonly float MAX_DENSITY = 22.5f;

    // Render the shape for a physics body
    public static void RenderPhysicsObject(PhysicsBody2D body, Color color)
    {
        // Get world transform and shape
        Vector2 position = body.Transform.Position;
        float rotation = body.Transform.Rotation;
        Vector2 scale = body.Transform.Scale;

        float width = body.Dimensions.Width;
        float height = body.Dimensions.Height;
        float radius = body.Dimensions.Radius;
        Vector2 size = new Vector2(width, height);

        // Use the raylib draw methods to render the shape for an object

        switch(body.Shape)
        {
            case ShapeType.Box:
                // Calculate scaled size
                Vector2 scaledSize = size * scale;

                // Calculate position adjustment
                Vector2 adjustmentBox = new Vector2((scaledSize.X - size.X) / 2, (scaledSize.Y - size.Y) / 2);

                // Draw the rectangle
                Raylib.DrawRectanglePro(new Rectangle(position - adjustmentBox, scaledSize), new Vector2(width / 2, height / 2), rotation, color);
                break;

            case ShapeType.Circle:

                // Draw the circle
                Raylib.DrawCircleV(position, radius, color); ;
                break;

            default: throw new Exception("[ERROR]: Invalid ShapeType");
        }
    }

    // Creates a Circle RigidBody
    public static void CreateRigidBody(Vector2 position, Vector2 scale, float density, float restitution,
        float radius, out RigidBody2D body2D)
    {
        body2D = null;
        string errorMessage;

        // Calculate the area for the rigid body
        float area = MathF.PI * radius * radius;

        errorMessage = ValidateParameters(area, density);

        // Exit function if there is an error
        if (!string.IsNullOrEmpty(errorMessage))
            throw new Exception(errorMessage);

        // Keep restitution in valid range
        restitution = Math.Clamp(restitution, 0.0f, 1.0f);

        // For Any Object, Mass = Volume * Denisty
        // Where Volume = Area * Depth in 3D space
        // For 2D plane, we can assume depth to be 1
        // Convert mass into kg
        float mass = (area * density) / 1000;

        List<Component> components = new List<Component>
        {
            new Gravity(),
            new LinMotion()
        };

        // Create a rigid body 
        body2D = new RigidCircle2D(position, scale, mass, density, area, restitution, radius, components);
    }

    // Creates a Box RigidBody
    public static void CreateRigidBody(Vector2 position, float rotation, Vector2 scale, float density, float restitution,
    float width, float height, out RigidBody2D body2D)
    {

        body2D = null;
        string errorMessage;

        // Calculate the area for the rigid body
        float area = width * height;

        errorMessage = ValidateParameters(area, density);

        // Exit function if there is an error
        if (!string.IsNullOrEmpty(errorMessage))
            throw new Exception(errorMessage);

        // Keep restitution in valid range
        restitution = Math.Clamp(restitution, 0.0f, 1.0f);

        // For Any Object, Mass = Volume * Denisty
        // Where Volume = Area * Depth in 3D space
        // For 2D plane, we can assume depth to be 1
        // Convert mass into kg
        float mass = (area * density) / 1000;

        List<Component> components = new List<Component>
        {
            new Gravity(),
            new LinMotion()
        };

        // Create a rigid body 
        body2D = new RigidBox2D(position, rotation, scale, mass, density, area, restitution, width, height, components);
    }

    // Creates a Circle StaticBody
    public static void CreateStaticBody(Vector2 position, Vector2 scale, float restitution,
    float radius, out StaticBody2D body2D)
    {
        body2D = null;
        string errorMessage;

        // Calculate the area for the static body
        float area = MathF.PI * radius * radius;

        errorMessage = ValidateParameters(area);

        // Exit function if there is an error
        if (!string.IsNullOrEmpty(errorMessage))
            throw new Exception(errorMessage);

        // Keep restitution in valid range
        restitution = Math.Clamp(restitution, 0.0f, 1.0f);

        // Static body has infinite mass
        float mass = float.MaxValue;

        // Create a static body
        body2D = new StaticCircle2D(position, scale, mass, restitution, area, radius);

    }

    // Creates a Box StaticBody
    public static void CreateStaticBody(Vector2 position, float rotation, Vector2 scale, float restitution,
    float width, float height, out StaticBody2D body2D)
    {
        body2D = null;
        string errorMessage;

        // Calculate the area for the static body
        float area = width * height;

        errorMessage = ValidateParameters(area);
            
        // Exit function if there is an error
        if (!string.IsNullOrEmpty(errorMessage)) 
            throw new Exception(errorMessage);

        // Keep restitution in valid range
        restitution = Math.Clamp(restitution, 0.0f, 1.0f);

        // Static body has infinite mass
        float mass = float.MaxValue;

        // Create a static body
        body2D = new StaticBox2D(position, rotation, scale, mass, area, restitution, width, height);
    }

    private static string ValidateParameters(float area, float density = 0)
    {
        string errorMessage = string.Empty;

        if (area < MIN_BODY_SIZE || area > MAX_BODY_SIZE)
            errorMessage = area < MIN_BODY_SIZE ? $"[ERROR]: Body area is too small, Minimum Area: {MIN_BODY_SIZE}" :
                           $"[ERROR]: Body area is too large, Maximum Area: {MAX_BODY_SIZE}";

        if (density != 0 && (density < MIN_DENSITY || density > MAX_DENSITY))
            errorMessage += errorMessage != string.Empty ? Environment.NewLine : string.Empty +
                            (density < MIN_DENSITY ? $"[ERROR]: Body density is too low, Minimum Density: {MIN_DENSITY}" :
                                                     $"[ERROR]: Body density is too high, Maximum Density: {MAX_DENSITY}");

        return errorMessage;
    }

    // Implement all physics stuff
    public static void HandlePhysics(List<PhysicsBody2D> bodies)
    {
        List<CollisionManifold> contacts = new List<CollisionManifold>();
        List<Vector2> contactPoints = new List<Vector2>();

        float accumulator = 0f;
        float timestep = 1f / 60f;

        contactPoints.Clear();

        // Make sure loop runs only once per frame
        while (accumulator < timestep)
        {
            contacts.Clear();

            for (int i = 0; i < bodies.Count; i++)
            {
                PhysicsBody2D bodyA = bodies[i];

                for (int j = i + 1; j < bodies.Count; j++)
                {
                    PhysicsBody2D bodyB = bodies[j];
                    Vector2 normal;
                    float depth;

                    // Check if objects may be colliding
                    if (!CollisionDetection.AABBIntersection(bodyA.GetAABB(), bodyB.GetAABB()))
                        continue;

                    // Detect collision and add contact points
                    if (CollisionDetection.CheckCollision(bodyA, bodyB, out normal, out depth))
                    {
                        CollisionHelper.FindContactPoints(bodyA, bodyB, out Vector2 contactP1, out Vector2 contactP2, out int contactCount);
                        CollisionManifold contact = new CollisionManifold(bodyA, bodyB, normal, depth, contactP1, contactP2, contactCount);

                        contacts.Add(contact);
                    }
                }
            }

            // Resolve collision at contact points
            foreach (CollisionManifold contact in contacts)
            {
                CollisionResolution.ResolveCollision(in contact);

                if (contact.CONTACT_COUNT > 0 && !contactPoints.Contains(contact.CONTACT_P1))
                {
                    contactPoints.Add(contact.CONTACT_P1);

                    if (contact.CONTACT_COUNT > 1 && !contactPoints.Contains(contact.CONTACT_P2))
                    {
                        contactPoints.Add(contact.CONTACT_P2);
                    }

                    // Drawing contact points for debugging
                    Raylib.DrawRectangle((int)contact.CONTACT_P1.X, (int)contact.CONTACT_P1.Y, 12, 12, Color.Orange);
                }
            }

            // Update body
            foreach (PhysicsBody2D body in bodies)
            {
                if (body is RigidBody2D)
                    body.RunComponents();

                CollisionHelper.UpdateCollisionState(body, bodies);
            }

            accumulator += timestep;
        }
    }
}
