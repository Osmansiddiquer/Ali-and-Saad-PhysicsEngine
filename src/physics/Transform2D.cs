using System.Numerics;

namespace GameEngine.src.physics;
public class Transform2D
{
    // World transform 
    public Vector2 Translation { get; private set; }
    public float Rotation { get; private set; }

    // Constructor
    public Transform2D(Vector2 translation, float rotation)
    {
        Rotation = rotation;
        Translation = translation;
    }

    // Move object
    internal void Translate(Vector2 direction) { Translation += direction; }
    
    // Rotate object
    internal void Rotate(float angle) { Rotation += angle; }
}
