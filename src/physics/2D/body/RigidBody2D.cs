using PhysicsEngine.src.physics._2D;
using System.Numerics;

namespace PhysicsEngine.src.body;

public class RigidBody2D : PhysicsBody2D
{


    // Force applied to the body
    public Vector2 Force;

    // Vertices (For collision handling)
    private readonly Vector2[]? vertices;
    private Vector2[]? transformedVertices;
    private bool verticesUpdateRequired;

    public readonly int[]? Triangles;

    // Constructor
    public RigidBody2D(Vector2 position, float rotation, Vector2 scale, float mass, float density, float area,
        float restitution, float radius, float width, float height, ShapeType shape)
    {
        Transform = new Transform2D(position, rotation, scale);
        Dimensions = new Dimensions2D(radius, width, height);
        Substance = new Substance2D(mass, density, area, restitution, false);

        Shape = shape;

        LinVelocity = Vector2.Zero;
        RotVelocity = 0f;

        Force = Vector2.Zero;

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

    public void Motion()
    {
        LinVelocity += Force / Substance.Mass;

        Transform.Translate(LinVelocity);
        Transform.Rotate(RotVelocity);
        
        verticesUpdateRequired = true;

        Force = Vector2.Zero;
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

