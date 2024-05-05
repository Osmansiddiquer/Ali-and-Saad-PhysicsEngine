using Raylib_cs;

namespace GameEngine.src.helper;

public struct Animation
{
    public Texture2D atlas;
    public int framesPerSecond;
    public List<Rectangle> rectangles;

    public Animation(Texture2D atlas, int framesPerSecond, List<Rectangle> rectangles)
    {
        this.atlas = atlas;
        this.framesPerSecond = framesPerSecond;
        this.rectangles = rectangles;
    }
}