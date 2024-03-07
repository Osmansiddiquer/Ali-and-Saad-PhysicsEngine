using PhysicsEngine.src.physics._2D;
using PhysicsEngine.src.world;
using Raylib_cs;
using System.Numerics;

namespace PhysicsEngine.src.body;

public enum ShapeType
{
    Circle, Box
}

public class PhysicsBody2D : PhysicsWorld2D
{
   
    public ShapeType Shape;

    public Transform2D Transform;
    public Dimensions2D Dimensions;
    public Substance2D Substance;

    // Velocity of the body
    public Vector2 LinVelocity;
    public float RotVelocity;

    // Vertices (For collision handling)
    protected Vector2[]? vertices;
    protected Vector2[]? transformedVertices;
    protected bool verticesUpdateRequired;

    protected int[]? Triangles;

    public static bool CreateRigidBody(Vector2 position, float rotation, Vector2 scale, float density, float restitution, ShapeType shape, 
            float radius, float width, float height, out RigidBody2D body2D, out string errorMessage)
    {
        body2D = null;
        errorMessage = string.Empty;

        // Calculate the area for the rigid body
        float area = CalculateArea(shape, radius, width, height, out errorMessage);

        // Check if dimensions and density are within range
        if (area < MIN_BODY_SIZE || area > MAX_BODY_SIZE || density < MIN_DENSITY || density > MAX_DENSITY)
        {
            errorMessage = area < MIN_BODY_SIZE ? $"Body area is too small, Minimum Area: {MIN_BODY_SIZE}" :
                           area > MAX_BODY_SIZE ? $"Body area is too large, Maximum Area: {MAX_BODY_SIZE}" :
                           density < MIN_DENSITY ? $"Body density is too low, Minimum Density: {MIN_DENSITY}" :
                                                   $"Body density is too high, Maximum Density: {MAX_DENSITY}";
        }

        // Exit function if there is an error
        if (errorMessage != string.Empty) return false;

        // Keep restitution in valid range
        restitution = Math.Clamp(restitution, 0.0f, 1.0f);

        // For Any Object, Mass = Volume * Denisty
        // Where Volume = Area * Depth in 3D space
        // For 2D plane, we can assume depth to be 1
        float mass = area * density;

        GravityComponent gravityComponent = new GravityComponent();
        List<Component> components = new List<Component>();
        components.Add(gravityComponent);

        // Create a rigid body 
        body2D = shape == ShapeType.Circle ?
                 new RigidBody2D(position, rotation, scale, mass, density, area, restitution, radius, 0f, 0f, ShapeType.Circle, components) :
                 new RigidBody2D(position, rotation, scale, mass, density, area, restitution, 0f, width, height, ShapeType.Box, components);

        return true;
    }

    public static bool CreateStaticBody(Vector2 position, float rotation, Vector2 scale, float restitution, ShapeType shape, 
        float radius, float width, float height, out StaticBody2D body2D, out string errorMessage)
    {
        body2D = null;
        errorMessage = string.Empty;

        // Calculate the area for the static body
        float area = CalculateArea(shape, radius, width, height, out errorMessage);

        // Check if dimensions within range
        if (area < MIN_BODY_SIZE || area > MAX_BODY_SIZE)
        {
            errorMessage = area < MIN_BODY_SIZE ? $"Body area is too small, Minimum Area: {MIN_BODY_SIZE}" :
                           $"Body area is too large, Maximum Area: {MAX_BODY_SIZE}";
        }

        // Exit function if there is an error
        if (errorMessage != string.Empty) return false;


        // Keep restitution in valid range
        restitution = Math.Clamp(restitution, 0.0f, 1.0f);

        // Static body has infinite mass
        float mass = 1f / 0;

        // Create a static body
        body2D = shape == ShapeType.Circle ?
                 new StaticBody2D(position, rotation, scale, mass, restitution, area, radius, 0f, 0f, ShapeType.Circle) :
                 new StaticBody2D(position, rotation, scale, mass, area, restitution, 0f, width, height, ShapeType.Box);

        return true;
    }

    private static float CalculateArea(ShapeType shapeType, float radius, float width, float height, out string errorMessage)
    {
        errorMessage = string.Empty;
        float area = 0f;

        switch (shapeType)
        {
            case ShapeType.Circle:
                if (radius <= 0f)
                {
                    errorMessage = "Invalid radius for circle.";
                    return 0f;
                }
                area = MathF.PI * radius * radius;
                break;
            case ShapeType.Box:
                if (width <= 0f || height <= 0f)
                {
                    errorMessage = "Invalid dimensions for box.";
                    return 0f;
                }
                area = width * height;
                break;
            default:
                errorMessage = "Invalid shape type.";
                return 0f;
        }

        return area;
    }


    public Vector2[] GetTransformedVertices()
    {
        if (verticesUpdateRequired)
        {
            Vector2 position = Transform.Position;
            float rotation = Transform.Rotation * (float)MathF.PI / 180f;
            Vector2 scale = Transform.Scale;

            // Create separate matrices for individual transformations
            Matrix3x2 translationMatrix = Matrix3x2.CreateTranslation(position);
            Matrix3x2 rotationMatrix = Matrix3x2.CreateRotation(rotation);
            //Matrix3x2 scalingMatrix = Matrix3x2.CreateScale(scale);

            // Combine transformations in desired order
            Matrix3x2 transformationMatrix = rotationMatrix * translationMatrix;

            // Update transformed vertices using the combined matrix
            for (int i = 0; i < vertices.Length; i++)
            {
                transformedVertices[i] = Vector2.Transform(vertices[i], transformationMatrix);
            }
        }

        verticesUpdateRequired = false;
        return transformedVertices;
    }

    protected void MapVertices(ShapeType shape)
    {
        // Create vertices for box shape
        if (shape is ShapeType.Box)
        {
            vertices = CreateVerticesBox(Dimensions.Width, Dimensions.Height);
            transformedVertices = new Vector2[vertices.Length];

            Triangles = CreateTrianglesBox();
        }

        // No vertices for circle
        else
        {
            vertices = null;
            transformedVertices = null;

            Triangles = null;
        }
    }

    // Create triangles for the box shape
    protected static int[] CreateTrianglesBox()
    {
        // A box has 2 triangles, so 6 points
        int[] triangles = new int[6];

        // Triangle 1
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        // Trinagle 2
        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        return triangles;
    }

    // Create vertices for the box shape
    protected static Vector2[] CreateVerticesBox(float width, float height)
    {
        // Sides
        float left = -width / 2f;
        float right = left + width;

        float bottom = -height / 2f;
        float top = bottom + height;

        // Array of vertices (stored as 2D vectors)
        Vector2[] vertices = new Vector2[4];

        // Top vertices
        vertices[0] = new Vector2(left, top);
        vertices[1] = new Vector2(right, top);

        // Bottom vertices
        vertices[2] = new Vector2(right, bottom);
        vertices[3] = new Vector2(left, bottom);

        return vertices;

    }

}

