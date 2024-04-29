using GameEngine.src.physics.component;
using Raylib_cs;
using System.Numerics;

namespace GameEngine.src.physics.body;

internal enum PlayerStates
{
    Idle,
    Running,
    Jumping,
    Falling,
    Crouching,
    Dead
}

internal struct PlayerAnimation
{
    Texture2D[] textureFile;
    int frameCount;
    int currentFrame;
    int frameSpeed;
}
public class PlayerBody2D : RigidBox2D
{
    public PlayerBody2D(Vector2 position, float rotation, Vector2 scale, float width, float height, List<Component> components) :
        base(position, rotation, scale, 0.985f * width * height, 0.985f, width * height, 0f, width, height, components) { }
}

