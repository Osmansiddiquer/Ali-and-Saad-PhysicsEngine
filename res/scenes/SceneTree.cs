using GameEngine.src.input;
using GameEngine.src.main;
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

        InputMap.AssignKey("one", KeyboardKey.One);
        InputMap.AssignKey("two", KeyboardKey.Two);
        InputMap.AssignKey("three", KeyboardKey.Three);
        InputMap.AssignKey("four", KeyboardKey.Four);
    }

    public static void Update(double delta)
    {
        if (InputMap.IsKeyPressed("one"))
            currentScene = new CollisionTest();

        else if (InputMap.IsKeyPressed("two"))
            currentScene = new ProjectileTest();

        else if (InputMap.IsKeyPressed("three"))
            currentScene = new TilemapTest();

        else if (InputMap.IsKeyPressed("four"))
            currentScene = new PlayerTest();

        currentScene.Update(delta);
            
    }
}