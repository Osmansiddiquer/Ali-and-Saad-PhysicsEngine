using PhysicsEngine.src.components;
using Raylib_cs;
using System.Numerics;

namespace PhysicsEngine.src.physics._2D.body;
public class PlayerBody2D : RigidBox2D {

    public static Input InputMap { get; private set; }
    public PlayerBody2D(Vector2 position, float rotation, Vector2 scale, float width, float height, List<Component> components) : 
        base(position, rotation, scale, 70, 0.985f, width * height, 0f, width, height, components) 
    {
        InputMap = new Input();
    }

}

