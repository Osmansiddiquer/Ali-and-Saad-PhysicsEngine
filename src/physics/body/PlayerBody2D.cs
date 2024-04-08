using GameEngine.src.input;
using GameEngine.src.physics.component;
using System.Numerics;

namespace GameEngine.src.physics.body;
public class PlayerBody2D : RigidBox2D
{

    public static InputMap Input { get; private set; }
    public PlayerBody2D(Vector2 position, float rotation, Vector2 scale, float width, float height, List<Component> components) :
        base(position, rotation, scale, 70, 0.985f, width * height, 0f, width, height, components)
    {
        Input = new InputMap();
    }

}

