using PhysicsEngine.src.physics._2D.collision;
using System.Numerics;

#pragma warning disable CS8618 // Non nullable field must have non null value when exiting constructor.

namespace PhysicsEngine.src.physics._2D.body;

public enum ShapeType
{
    Circle, Box
}

public abstract class PhysicsBody2D
{
    public string Name;
    public ShapeType Shape { get; protected set; }
    public Transform2D Transform { get; protected set; }
    public Dimensions2D Dimensions { get ; protected set; }
    public Substance2D Substance { get; protected set; }

    protected Vector2[] Vertices;
    protected Vector2[] TransformedVertices;
    protected AxisAlignedBoundingBox AABB;

    public bool VerticesUpdateRequired { get; internal set; }
    public bool AABBUpdateRequired { get; internal set; }

    // Current collision state of object
    private bool isOnFloor;
    public bool IsOnFloor
    {
        get { return isOnFloor; }
        internal set { isOnFloor = value; }
    }

    private bool isOnCeiling;
    public bool IsOnCeiling
    {
        get { return isOnCeiling; }
        internal set { isOnCeiling = value; }
    }

    private bool isOnWallR;
    public bool IsOnWallR
    {
        get { return isOnWallR; }
        internal set { isOnWallR = value; }
    }

    private bool isOnWallL;
    public bool IsOnWallL
    {
        get { return isOnWallL; }
        internal set { isOnWallL = value; }
    }

    // Linear motion attributes
    public Vector2 LinVelocity { get; internal set; }
    public Vector2 Normal { get; internal set; }

    // Rotational motion attributes
    public float RotVelocity { get; internal set; }
    public float MomentOfInertia { get; protected set; }

    public PhysicsBody2D(Vector2 position, float rotation, Vector2 scale)
    {
        Transform = new Transform2D(position, rotation, scale);
        Dimensions = new Dimensions2D();
        Substance = new Substance2D();

        VerticesUpdateRequired = true;
        AABBUpdateRequired = true;
    }

    // Calculate new position of vertices after transformation
    internal Vector2[] GetTransformedVertices()
    {
        if (VerticesUpdateRequired)
        {
            Vector2 position = Transform.Position;
            float rotation = Transform.Rotation * MathF.PI / 180f;
            Vector2 scale = Transform.Scale;

            // Create separate matrices for individual transformations
            Matrix3x2 translationMatrix = Matrix3x2.CreateTranslation(position);
            Matrix3x2 rotationMatrix = Matrix3x2.CreateRotation(rotation);
            Matrix3x2 scalingMatrix = Matrix3x2.CreateScale(scale);

            // Combine transformations in desired order
            Matrix3x2 transformationMatrix = scalingMatrix * rotationMatrix * translationMatrix;

            // Update transformed vertices using the combined matrix n bn
            for (int i = 0; i < Vertices.Length; i++)
                TransformedVertices[i] = Vector2.Transform(Vertices[i], transformationMatrix);
        }

        VerticesUpdateRequired = false;
        return TransformedVertices;
    }

    internal AxisAlignedBoundingBox GetAABB()
    {
        if (AABBUpdateRequired)
        {
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

            AABB = new AxisAlignedBoundingBox(minX, minY, maxX, maxY);
        }

        AABBUpdateRequired = false;
        return AABB;
    }

    // Map the vertices to a box shape
    protected void MapVerticesBox()
    {
        Vertices = CreateVerticesBox(Dimensions.Width, Dimensions.Height);
        TransformedVertices = new Vector2[Vertices.Length];
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

    // Methods to be overridden
    internal virtual void RunComponents() { }
    internal virtual void ApplyForce(Vector2 amount) { }
    internal virtual void ProjectileHit(PhysicsBody2D body) { }
    public virtual void Translate(Vector2 amount) { }
    public virtual void Rotate(float angle) { }
    public virtual void Scale(Vector2 amount) { }
}

