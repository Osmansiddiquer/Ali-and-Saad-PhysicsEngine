using PhysicsEngine.src.body;
using Raylib_cs;
using System.Numerics;


namespace PhysicsEngine.src.physics._2D.body;

public static class RenderBody2D
{
    // Render the shape for a physics body
    public static void RenderPhysicsObject(PhysicsBody2D body, Color color)
    {
        // Get world transform and shape
        Vector2 position = body.Transform.Position;
        float rotation = body.Transform.Rotation;


        float width = body.Dimensions.Width;
        float height = body.Dimensions.Height;
        float radius = body.Dimensions.Radius;
        Vector2 size = new Vector2(width, height);

        // Use the raylib draw methods to render the shape for an object

        if (body.Shape is ShapeType.Box) { 
            Raylib.DrawRectanglePro(new Rectangle(position, size), new Vector2(width / 2, height / 2), rotation, color); 
        }

        else {
            Raylib.DrawCircleV(position, radius, color);
        }
        
    }

}
