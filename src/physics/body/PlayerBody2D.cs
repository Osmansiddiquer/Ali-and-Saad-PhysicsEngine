using GameEngine.src.input;
using GameEngine.src.physics.component;
using Raylib_cs;
using System.Numerics;

namespace GameEngine.src.physics.body;

internal enum PlayerStates
{
    IDLE,
    WALK,
    JUMP,
    FALL,
    CROUCH,
    DIE
}

internal struct Animation
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
public class PlayerBody2D : RigidBox2D
{
    internal PlayerStates State { get; set; }

    private List<Animation> animations;
    public PlayerBody2D(Vector2 position, float rotation, float width, float height, List<Component> components) :
        base(position, rotation, 0.985f * width * height, 0.985f, width * height, 0f, width, height, components) 
    {
        // Initialize the player
        State = PlayerStates.IDLE;
        animations = new List<Animation>();

        // Initialize the player animations
        createAnimations();

    }

    private void DrawPlayerAnimation(Rectangle dest, Vector2 origin, float rotation, Color tint)
    {
        Animation currAnimation = animations[0];
        if (InputMap.IsKeyDown("left") || InputMap.IsKeyDown("right"))
        {
            currAnimation = animations[1];
        }
        else
        {
            currAnimation = animations[0];
        }   

        int index = (int)(Raylib.GetTime() * currAnimation.framesPerSecond) % currAnimation.rectangles.Count;
        Raylib.DrawTexturePro(currAnimation.atlas, currAnimation.rectangles[index], dest, origin, rotation, tint);
    }

    public void DrawPlayer()
    {
        DrawPlayerAnimation(new Rectangle((Transform.Translation + new Vector2(-90, -180)), 180, 360), new Vector2(0, 0), 0, Color.White);
    }

    private void createAnimations()
    {
        Texture2D playerAtlas = Raylib.LoadTexture("C:/Users/saadk/Desktop/NUST/Semester 2/Object Oriented Programming/End Semester Project/sprites/Hero Knight/Sprites/Idle.png");
        List<Rectangle> rectangles = new List<Rectangle>();
        for (int i = 0; i < 11; i++)
        {
            rectangles.Add(new Rectangle((i) * 180, 0, 180, 180));
        }
        Animation idle = new Animation(playerAtlas, 10, rectangles);
        animations.Add(idle);

        playerAtlas = Raylib.LoadTexture("C:/Users/saadk/Desktop/NUST/Semester 2/Object Oriented Programming/End Semester Project/sprites/Hero Knight/Sprites/Run.png");
        for (int i = 0; i < 8; i++)
        {
            rectangles.Add(new Rectangle((i) * 180, 0, 180, 180));
        }
        Animation run = new Animation(playerAtlas, 10, rectangles);
        animations.Add(run);
    }
}

