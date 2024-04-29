using GameEngine.src.input;
using Raylib_cs;

namespace GameEngine.res.scenes;

public static class SceneTree
{
    internal static int scene;

    static SceneTree()
    {
        scene = 0;

        InputMap.AssignKey("one", KeyboardKey.One);
        InputMap.AssignKey("two", KeyboardKey.Two);
        InputMap.AssignKey("three", KeyboardKey.Three);
    }

    public static void Update(double delta)
    {
        if (InputMap.IsKeyPressed("one"))
            scene = 0;


        else if (InputMap.IsKeyPressed("two"))
            scene = 1;

        else if (InputMap.IsKeyPressed("three"))
            scene = 2;

        switch (scene)
        {
            case 0:
                CollisionTest.Update(delta);
                break;

            case 1:
                ProjectileTest.Update(delta);
                break;

            case 2:
                TilemapTest.Update(delta);
                break;

            default:
                break;
        }
    }


}