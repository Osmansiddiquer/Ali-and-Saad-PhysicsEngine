using PhysicsEngine.src.physics._2D;
using PhysicsEngine.src.world;
using Raylib_cs;
using System.Numerics;

namespace PhysicsEngine.src.body;

public enum ShapeType
{
    Circle, Box
}

public class PhysicsBody2D : World2D
{
   
    public ShapeType Shape;

    public Transform2D Transform;
    public Dimensions2D Dimensions;
    public Substance2D Substance;

    // Whether the object is static or rigid
    public readonly bool IsStatic;

    // Create a physics body with shape circle
    public static bool CreateCircleBody(Vector2 position, Vector2 scale, Color color, float density, float restitution, 
                            float radius, bool isStatic, out PhysicsBody2D body2D, out string errorMessage)
    {
        body2D = null;
        errorMessage = string.Empty;

        // Calculate area
        float area = MathF.PI * radius * radius;

        // Check if dimensions and density are within acceptable ranges
        if (area < MIN_BODY_SIZE || area > MAX_BODY_SIZE || density < MIN_DENSITY || density > MAX_DENSITY)
        {
            if (area < MIN_BODY_SIZE)
                errorMessage = $"Body area is too small, Minimum Area: {MIN_BODY_SIZE}";

            else if (area > MAX_BODY_SIZE)
                errorMessage = $"Body area is too large, Maximum Area: {MAX_BODY_SIZE}";

            else if (density < MIN_DENSITY)
                errorMessage = $"Body density is too low, Minimum Density: {MIN_DENSITY}";

            else
                errorMessage = $"Body density is too high, Maximum Density: {MAX_DENSITY}";

            return false;
        }

        restitution = Math.Clamp(restitution, 0.0f, 1.0f);

        // Mass = Area * Depth * Density
        // Let Depth = 1f for 2D
        // Assuming a circle in 2D is a cylinder in 3D
        float mass = area * 1f * density;

        // Create a new physics body
        body2D = !isStatic
            ? new RigidBody2D(position, 0f, scale, mass, density, area, restitution, radius, 0f, 0f, ShapeType.Circle)
            : new StaticBody2D(position, 0f, scale, area, radius, 0f, 0f, ShapeType.Circle);

        return true;
    }

    // Create a physics body with shape box
    public static bool CreateBoxBody(Vector2 position, float rotation, Vector2 scale, Color color, float density, float restitution,
                                  float width, float height, bool isStatic, out PhysicsBody2D body2D, out string errorMessage)
    {
        body2D = null;
        errorMessage = string.Empty;

        // Calculate area
        float area = width * height;

        // Check if dimensions and density are within acceptable ranges
        if (area < MIN_BODY_SIZE || area > MAX_BODY_SIZE || density < MIN_DENSITY || density > MAX_DENSITY)
        {
            if (area < MIN_BODY_SIZE)
                errorMessage = $"Body area is too small, Minimum Area: {MIN_BODY_SIZE}";
            else if (area > MAX_BODY_SIZE)
                errorMessage = $"Body area is too large, Maximum Area: {MAX_BODY_SIZE}";
            else if (density < MIN_DENSITY)
                errorMessage = $"Body density is too low, Minimum Density: {MIN_DENSITY}";
            else
                errorMessage = $"Body density is too high, Maximum Density: {MAX_DENSITY}";

            return false;
        }

        restitution = Math.Clamp(restitution, 0.0f, 1.0f);

        // Mass = Area * Depth * Density
        // Let Depth = 1f for 2D
        // Assuming a box in 2D is a cuboid in 3D
        float Mass = area * 1f * density;

        // Normalize rotation to be between 0 and 360
        rotation = rotation % 360;

        // Create a new physics body
        body2D = !isStatic
            ? new RigidBody2D(position, rotation, scale, Mass, density, area, restitution, 0f, width, height, ShapeType.Box)
            : new StaticBody2D(position, rotation, scale, area, 0f, width, height, ShapeType.Box);

        return true;
    }
}

