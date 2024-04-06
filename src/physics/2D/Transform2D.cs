using System.Numerics;

namespace PhysicsEngine.src.physics._2D;
public class Transform2D
{
    // World transform 
    public Vector2 Position { get; private set; }
    public Vector2 Scale { get; private set; }
    public float Rotation { get; private set; }

    // Constructor
    public Transform2D(Vector2 position, float rotation, Vector2 scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }

    // Move object
    internal void Translate(Vector2 direction) { Position += direction; }
    
    // Rotate object
    internal void Rotate(float angle) { Rotation += angle; }

    // Scale object
    internal void Scaling(Vector2 scale) { Scale *= scale; }

}
