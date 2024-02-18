using PhysicsEngine.src.physics;
using Raylib_cs;
using System.Numerics;

namespace PhysicsEngine.src.body;

public class RigidBody2D : PhysicsBody2D
{
    // Velocity of the body
    private float LinVelocity;
    private float RotVelocity;

    // Physics attributes
    public readonly float Mass;
    public readonly float Density;
    public readonly float Area;
    public readonly float Restitution;

    // Vertices (For collision handling)
    private readonly Vector2[]? vertices;
    private Vector2[]? transformedVertices;

    public readonly int[]? Triangles;

    public bool verticesUpdateRequired;

    // Constructor
    public RigidBody2D(Vector2 position, float rotation, Vector2 scale, float mass, float density, float area, 
        float restitution, float radius, float width, float height, ShapeType shape) 
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;

        LinVelocity = 0f;
        RotVelocity = 0f;

        Mass = mass;
        Density = density;
        Area = area;
        Restitution = restitution;

        Shape = shape;

        Area = area;
        Radius = radius;
        Width = width;
        Height = height;

        // Create vertices for box shape
        if (shape is ShapeType.Box) {
            vertices = CreateVerticesBox(Width, Height);
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
    public void Move(Vector2 direction)
    {
        Position += direction;
        verticesUpdateRequired = true;
    }

    public Vector2[] GetTransformedVertices()
    {
        if (verticesUpdateRequired)
        {
            // Create separate matrices for individual transformations
            Matrix3x2 translationMatrix = Matrix3x2.CreateTranslation(Position);
            Matrix3x2 rotationMatrix = Matrix3x2.CreateRotation(Rotation);
            Matrix3x2 scalingMatrix = Matrix3x2.CreateScale(Scale);

            // Combine transformations in desired order
            Matrix3x2 transformationMatrix = translationMatrix * rotationMatrix * scalingMatrix;

            // Update transformed vertices using the combined matrix
            for (int i = 0; i < vertices.Length; i++)
            {
                transformedVertices[i] = Vector2.Transform(vertices[i], transformationMatrix);
            }
        }

        verticesUpdateRequired = false;
        return transformedVertices;
    }

    // Create triangles for the box shape
    private static int[] CreateTrianglesBox() 
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
    private static Vector2[] CreateVerticesBox(float width, float height) 
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

