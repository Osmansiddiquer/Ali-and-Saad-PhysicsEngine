using GameEngine.src.physics.body;
using System.Numerics;
using GameEngine.src.input;

namespace GameEngine.src.physics.component;

internal class PlayerMovement : Component
{
    public void RunComponent(RigidBody2D body, double delta)
    {
        MovePlayer(body, delta);
    }

    private void MovePlayer(RigidBody2D body, double delta)
    {
        // Move the player
        if (Input.IsKeyDown("left"))
        {
            body.LinVelocity = new Vector2(-1, body.LinVelocity.Y);
        }
        else if (Input.IsKeyDown("right"))
        {
            body.LinVelocity = new Vector2(1, body.LinVelocity.Y);
        }
        else
        {
            body.LinVelocity = new Vector2(0, body.LinVelocity.Y);
        }
    }
}


