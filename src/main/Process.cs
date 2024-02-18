using PhysicsEngine.res.scenes;
using Raylib_cs;

public static class Process
{
    public const int DEFAULT_SCRN_WDTH = 960;
    public const int DEFAULT_SCRN_HGHT = 640;
    private const string TITLE = "Physics Engine";

    public static bool DisplayFPS = true;
    public static bool Fullscreen = false;
    public static bool EnableVSync = true;
    public static int MaxFPS = 60;

    // Define percentages for element positions and sizes
    private const float FPS_POSITION_X_PERCENTAGE = 0.9f;
    private const float FPS_POSITION_Y_PERCENTAGE = 0.0375f;

    private static float fpsPositionX;
    private static float fpsPositionY;

    public static void Start()
    {
        Init(); // Initialize the program
        Loop(); // Process loop

        // Stop at the end of loop
        Stop();
    }

    // Initialization 
    private static void Init()
    {
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow); // Enable resizable window
        Raylib.InitWindow(DEFAULT_SCRN_WDTH, DEFAULT_SCRN_HGHT, TITLE); // Initialize Window
        Raylib.SetTargetFPS(MaxFPS); // Set max FPS

        // Begin simulation
        Debug.Ready();
    }

    // Game loop
    private static void Loop()
    {
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            // Calculate initial FPS position based on window size
            fpsPositionX = Raylib.GetScreenWidth() * FPS_POSITION_X_PERCENTAGE;
            fpsPositionY = Raylib.GetScreenHeight() * FPS_POSITION_Y_PERCENTAGE;

            // Show FPS to screen
            if (DisplayFPS)
            {
                // Draw FPS using calculated position
                Raylib.DrawFPS((int)fpsPositionX, (int)fpsPositionY);
            }

            // Update the simulation program
            Debug.Update((double)Raylib.GetFrameTime());

            Raylib.EndDrawing();
        }
    }

    // End Process (Obviously)
    private static void Stop()
    {
        Raylib.CloseWindow();
    }
}
