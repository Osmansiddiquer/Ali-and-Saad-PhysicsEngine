using GameEngine.src.physics.collision;
using System.Numerics;

#pragma warning disable CS8618 // Non nullable field must have non null value when exiting constructor.

namespace GameEngine.src.physics.body;
public enum ShapeType
{
    Circle, Box
}

public abstract class PhysicsBody2D
{
    public string Name;
    public ShapeType Shape { get; protected set; }

    // Physical properties of the body
    public Transform2D Transform { get; protected set; }
    public Dimensions2D Dimensions { get ; protected set; }
    public Material2D Material { get; protected set; }

    // Vertices and Bounding Boxes
    protected Vector2[] Vertices;
    protected Vector2[] TransformedVertices;
    protected AxisAlignedBoundingBox AABB;

    protected bool VerticesUpdateRequired;
    protected bool AABBUpdateRequired;

    // Current collision state
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

    public bool HandleCollision;

    // Linear motion attributes
    public Vector2 LinVelocity { get; internal set; }
    public float RotVelocity { get; internal set; }
    public float MomentOfInertia { get; protected set; }

    // Constructor
    public PhysicsBody2D(Vector2 position, float rotation, Vector2 scale)
    {
        // Initialize physical properties
        Transform = new Transform2D(position, rotation, scale);

        HandleCollision = true;

        VerticesUpdateRequired = true;
        AABBUpdateRequired = true;

        Ready();
    }

    // Calculate new position of vertices after transformation
    internal Vector2[] GetTransformedVertices()
    {
        if (VerticesUpdateRequired)
        {
            Vector2 position = Transform.Translation;
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

        VerticesUpdateRequired = false; // No further need to update vertices
        return TransformedVertices;
    }

    // Calculate new AABB after transformation
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
                // Calculate new min and max values for AABB
                case ShapeType.Box:
                    Vector2[] vertices = GetTransformedVertices();

                    // Find min and max position of edges using vertices
                    foreach (Vector2 vertex in vertices)
                    {
                        if (vertex.X < minX) minX = vertex.X;
                        if (vertex.Y < minY) minY = vertex.Y;

                        if (vertex.X > maxX) maxX = vertex.X;
                        if (vertex.Y > maxY) maxY = vertex.Y;
                    }

                    break;

                // Find min and max position fo edges using radius
                case ShapeType.Circle:
                    minX = Transform.Translation.X - Dimensions.Radius;
                    minY = Transform.Translation.Y - Dimensions.Radius;

                    maxX = Transform.Translation.X + Dimensions.Radius;
                    maxY = Transform.Translation.Y + Dimensions.Radius;

                    break;

                default: throw new Exception("[ERROR]: Invalid ShapeType");
            }

            AABB = new AxisAlignedBoundingBox(minX, minY, maxX, maxY);
        }

        AABBUpdateRequired = false; // No further need to update AABB
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

    internal void ResetCollisionState()
    {
        // Reset all collision-related properties to false 
        isOnCeiling = false;
        isOnFloor = false;
        isOnWallL = false;
        isOnWallR = false;
    }

    // Methods to be overridden
    internal virtual void RunComponents(double delta) { }
    public virtual void ProjectileHit(PhysicsBody2D body) { }
    public virtual void Translate(Vector2 direction) { }
    public virtual void Rotate(float angle) { }
    public virtual void Scale(Vector2 factor) { }
    public virtual void ApplyForce(Vector2 amount) { }

    public virtual void Update() { }

    public virtual void Ready() { }
}

