using System.Numerics;

namespace PhysicsEngine.src.physics._2D;
public class Transform2D
{
    // World transform 
    public Vector2 Translation { get; private set; }
    public Vector2 Scale { get; private set; }
    public float Rotation { get; private set; }

    // Constructor
    public Transform2D(Vector2 translation, float rotation, Vector2 scale)
    {
        Scale = scale;
        Rotation = rotation;
        Translation = translation;
    }

    // Move object
    internal void Translate(Vector2 direction) { Translation += direction; }
    
    // Rotate object
    internal void Rotate(float angle) { Rotation += angle; }

    // Scale object
    internal void Scaling(Vector2 factor) { Scale *= factor; }

}
