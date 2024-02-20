using PhysicsEngine.src.body;
using Raylib_cs;
using System.Numerics;


namespace PhysicsEngine.src.physics._2D.body;

public static class RenderBody2D
{
    public static void RenderPhysicsObject(PhysicsBody2D body, Color color)
    {
        Vector2 position = body.Transform.Position;
        float rotation = body.Transform.Rotation;

        float width = body.Dimensions.Width;
        float height = body.Dimensions.Height;
        Vector2 size = new Vector2(width, height);

        Raylib.DrawRectanglePro(new Rectangle(position, size), new Vector2(width / 2, height / 2), rotation, color);
    }

    // Access position and radius from the physics body

}
