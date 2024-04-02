using PhysicsEngine.src.physics._2D.collision;
using System.Numerics;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning disable CS8618 // Non nullable field must have non null value when exiting constructor.

namespace PhysicsEngine.src.physics._2D.body;

public enum ShapeType
{
    Circle, Box
}

public class PhysicsBody2D 
{
   // Identifier for shape type
    public ShapeType Shape;

    // Transformation, Dimensions and 
    // Physical Properties for the body
    public Transform2D Transform;
    public Dimensions2D Dimensions;
    public Substance2D Substance;

    // Velocity of the body
    public Vector2 LinVelocity;
    public float RotVelocity;

    // Vertices (For collision handling)
    protected Vector2[] vertices;
    protected int[] triangles;
    protected Vector2[] transformedVertices;
    protected AxisAlignedBoundingBox aabb;

    public bool VerticesUpdateRequired;
    public bool AABBUpdateRequired;

    // Constructor
    public PhysicsBody2D(Vector2 position, float rotation, Vector2 scale)
    {
        Transform = new Transform2D(position, rotation, scale);

        VerticesUpdateRequired = true;
        AABBUpdateRequired = true;
    }

    // Calculate new position of vertices after transformation
    public Vector2[] GetTransformedVertices()
    {
        if (VerticesUpdateRequired)
        {
            Vector2 position = Transform.Position;
            float rotation = Transform.Rotation * (float)MathF.PI / 180f;
            Vector2 scale = Transform.Scale;

            // Create separate matrices for individual transformations
            Matrix3x2 translationMatrix = Matrix3x2.CreateTranslation(position);
            Matrix3x2 rotationMatrix = Matrix3x2.CreateRotation(rotation);
            Matrix3x2 scalingMatrix = Matrix3x2.CreateScale(scale);

            // Combine transformations in desired order
            Matrix3x2 transformationMatrix = scalingMatrix * rotationMatrix * translationMatrix;

            // Update transformed vertices using the combined matrix n bn
            for (int i = 0; i < vertices.Length; i++)
                transformedVertices[i] = Vector2.Transform(vertices[i], transformationMatrix);
            
        }

        VerticesUpdateRequired = false;
        return transformedVertices;
    }

    public AxisAlignedBoundingBox GetAABB()
    {
        if (AABBUpdateRequired) {
            float minX = float.PositiveInfinity;
            float minY = float.PositiveInfinity;

            float maxX = float.NegativeInfinity;
            float maxY = float.NegativeInfinity;

            switch (Shape)
            {
                case ShapeType.Box:
                    Vector2[] vertices = GetTransformedVertices();

                    foreach (Vector2 vertex in vertices)
                    {
                        if (vertex.X < minX) minX = vertex.X;
                        if (vertex.Y < minY) minY = vertex.Y;

                        if (vertex.X > maxX) maxX = vertex.X;
                        if (vertex.Y > maxY) maxY = vertex.Y;
                    }

                    break;

                case ShapeType.Circle:
                    minX = Transform.Position.X - Dimensions.Radius;
                    minY = Transform.Position.Y - Dimensions.Radius;

                    maxX = Transform.Position.X + Dimensions.Radius;
                    maxY = Transform.Position.Y + Dimensions.Radius;

                    break;

                default: throw new Exception("[ERROR]: Invalid ShapeType");
            }

            aabb = new AxisAlignedBoundingBox(minX, minY, maxX, maxY);
        }
        
        AABBUpdateRequired = false;
        return aabb;
    }

    // Map the vertices to a box shape
    protected void MapVerticesBox()
    {
        vertices = CreateVerticesBox(Dimensions.Width, Dimensions.Height);
        transformedVertices = new Vector2[vertices.Length];

        triangles = CreateTrianglesBox();
    }

    // Set vertices to null for circle shape
    protected void MapVerticesCircle()
    {

        vertices = null;
        transformedVertices = null;

        triangles = null;
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

    // Methods to be overriden
    public virtual void RunComponents() { }
    public virtual void ApplyForce(Vector2 amount) { }

    public virtual void Translate(Vector2 amount) { }

    public virtual void Rotate(float angle) { }

    public virtual void Scale(Vector2 amount) {  }
}

