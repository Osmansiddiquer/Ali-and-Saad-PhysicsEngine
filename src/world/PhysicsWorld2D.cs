using Raylib_cs;
using System.Numerics;
using PhysicsEngine.src.components;
using PhysicsEngine.src.physics._2D.body;

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
    public static void CreateRigidBody(Vector2 position, float rotation, Vector2 scale, float density, float restitution,
        float radius, out RigidBody2D body2D)
    {
        body2D = null;
        string errorMessage;

        // Calculate the area for the rigid body
        float area = CalculateArea(radius, 0f, 0f, out errorMessage);

        // Check if dimensions and density are within range
        if (area < MIN_BODY_SIZE || area > MAX_BODY_SIZE || density < MIN_DENSITY || density > MAX_DENSITY)
        {
            errorMessage = area < MIN_BODY_SIZE ? $"[ERROR]: Body area is too small, Minimum Area: {MIN_BODY_SIZE}" :
                           area > MAX_BODY_SIZE ? $"[ERROR]: Body area is too large, Maximum Area: {MAX_BODY_SIZE}" :
                           density < MIN_DENSITY ? $"[ERROR]: Body density is too low, Minimum Density: {MIN_DENSITY}" :
                                                   $"[ERROR]: Body density is too high, Maximum Density: {MAX_DENSITY}";
        }

        // Exit function if there is an error
        if (errorMessage != string.Empty) throw new Exception(errorMessage);

        // Keep restitution in valid range
        restitution = Math.Clamp(restitution, 0.0f, 1.0f);

        // For Any Object, Mass = Volume * Denisty
        // Where Volume = Area * Depth in 3D space
        // For 2D plane, we can assume depth to be 1
        // Convert mass into kg
        float mass = (area * density) / 1000;

        List<Component> components = new List<Component>();
        Gravity gravityComponent = new Gravity();
        Motion motionComponent = new Motion();

        components.Add(gravityComponent);
        components.Add(motionComponent);

        // Create a rigid body 
        body2D = new RigidCircle2D(position, rotation, scale, mass, density, area, restitution, radius, components);
    }

    // Creates a Box RigidBody
    public static void CreateRigidBody(Vector2 position, float rotation, Vector2 scale, float density, float restitution,
    float width, float height, out RigidBody2D body2D)
    {

        body2D = null;

        string errorMessage;

        // Calculate the area for the rigid body
        float area = CalculateArea(0, width, height, out errorMessage);

        // Check if dimensions and density are within range
        if (area < MIN_BODY_SIZE || area > MAX_BODY_SIZE || density < MIN_DENSITY || density > MAX_DENSITY)
        {
            errorMessage = area < MIN_BODY_SIZE ? $"Body area is too small, Minimum Area: {MIN_BODY_SIZE}" :
                           area > MAX_BODY_SIZE ? $"Body area is too large, Maximum Area: {MAX_BODY_SIZE}" :
                           density < MIN_DENSITY ? $"Body density is too low, Minimum Density: {MIN_DENSITY}" :
                                                   $"Body density is too high, Maximum Density: {MAX_DENSITY}";
        }

        // Exit function if there is an error
        if (errorMessage != string.Empty) throw new Exception(errorMessage);

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
            new Motion()
        };

        // Create a rigid body 
        body2D = new RigidBox2D(position, rotation, scale, mass, density, area, restitution, width, height, components);


    }

    // Creates a Circle StaticBody
    public static void CreateStaticBody(Vector2 position, float rotation, Vector2 scale, float restitution,
    float radius, out StaticBody2D body2D)
    {
        body2D = null;
        string errorMessage;

        // Calculate the area for the static body
        float area = CalculateArea(radius, 0f, 0f, out errorMessage);

        // Check if dimensions within range
        if (area < MIN_BODY_SIZE || area > MAX_BODY_SIZE)
        {
            errorMessage = area < MIN_BODY_SIZE ? $"Body area is too small, Minimum Area: {MIN_BODY_SIZE}" :
                           $"Body area is too large, Maximum Area: {MAX_BODY_SIZE}";
        }

        // Exit function if there is an error
        if (errorMessage != string.Empty) throw new Exception(errorMessage);

        // Keep restitution in valid range
        restitution = Math.Clamp(restitution, 0.0f, 1.0f);

        // Static body has infinite mass
        float mass = float.MaxValue;

        // Create a static body
        body2D = new StaticCircle2D(position, rotation, scale, mass, restitution, area, radius);

    }

    // Creates a Box StaticBody
    public static void CreateStaticBody(Vector2 position, float rotation, Vector2 scale, float restitution,
    float width, float height, out StaticBody2D body2D)
    {
        body2D = null;
        string errorMessage;

        // Calculate the area for the static body
        float area = CalculateArea(0f, width, height, out errorMessage);

        // Check if dimensions within range
        if (area < MIN_BODY_SIZE || area > MAX_BODY_SIZE)
        {
            errorMessage = area < MIN_BODY_SIZE ? $"[ERROR]: Body area is too small, Minimum Area: {MIN_BODY_SIZE}" :
                           $"[ERROR]: Body area is too large, Maximum Area: {MAX_BODY_SIZE}";
        }

        // Exit function if there is an error
        if (errorMessage != string.Empty) throw new Exception(errorMessage);

        // Keep restitution in valid range
        restitution = Math.Clamp(restitution, 0.0f, 1.0f);

        // Static body has infinite mass
        float mass = float.MaxValue;

        // Create a static body
        body2D = new StaticBox2D(position, rotation, scale, mass, area, restitution, width, height);
    }

    private static float CalculateArea(float radius, float width, float height, out string errorMessage)
    {
        errorMessage = string.Empty;
        float area = 0f;

        // Calculate area for circle
        if (radius != 0f) {
            if (radius < 0f) errorMessage = "[ERROR]: Invalid Radius For Circle";
            else area = MathF.PI * radius * radius;           
        }

        else {
            if (height < 0f || width < 0f) errorMessage = "[ERROR]: Invalid Dimensions For Box";
            else area = width * height;
        }

        return area;
    }

    //public static bool CreatePlayerBody(Vector2 position, float rotation, Vector2 scale, float width, float height, Camera2D camera, out PlayerBody2D playerBody)
    //{
    //    List<Component> components = new List<Component>
    //    {
    //        new Gravity(),
    //        new Motion()
    //    };

    //    playerBody = new PlayerBody2D(position, rotation, scale, width, height, components, camera);

    //    return true;
    //}

}
