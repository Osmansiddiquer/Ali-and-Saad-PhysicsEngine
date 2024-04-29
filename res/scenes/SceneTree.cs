using Raylib_cs;

namespace GameEngine.res.scenes;

public static class SceneTree
{
    internal static int scene = 0;

    public static void Update(double delta)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.One))
            scene = 0;
        

        else if (Raylib.IsKeyPressed(KeyboardKey.Two))        
            scene = 1;

        switch (scene)
        {
            case 0:
                CollisionTest.Update(delta);
                break;

            case 1:
                ProjectileTest.Update(delta);
                break;

            default:
                break;
        }
    }


}