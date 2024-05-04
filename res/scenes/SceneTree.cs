using GameEngine.src.input;
using GameEngine.src.world;
using Raylib_cs;

namespace GameEngine.res.scenes;

public static class SceneTree
{
    internal static int scene;
    internal static World2D currentScene;

    static SceneTree()
    {
        scene = 0;
        currentScene = new CollisionTest();

        Input.AssignKey("one", KeyboardKey.One);
        Input.AssignKey("two", KeyboardKey.Two);
        Input.AssignKey("three", KeyboardKey.Three);
        Input.AssignKey("four", KeyboardKey.Four);
    }

    public static void Update(double delta)
    {
        if (Input.IsKeyPressed("one"))
            currentScene = new CollisionTest();

        else if (Input.IsKeyPressed("two"))
            currentScene = new ProjectileTest();

        else if (Input.IsKeyPressed("three"))
            currentScene = new TilemapTest();

        else if (Input.IsKeyPressed("four"))
            currentScene = new PlayerTest();

        currentScene.Update(delta);
            
    }
}