using System.Numerics;

namespace PhysicsEngine.src.physics._2D;
public class Transform2D
{
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
    public void Translate(Vector2 direction) { Position += direction; }
    public void Rotate(float angle) { Rotation += angle; }

    public void Scaling(Vector2 newScale) { Scale = newScale; }

}
