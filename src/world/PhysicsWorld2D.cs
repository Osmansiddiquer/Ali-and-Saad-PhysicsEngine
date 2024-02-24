namespace PhysicsEngine.src.world;
public class PhysicsWorld2D
{
    // Constraints
    protected static readonly float MIN_BODY_SIZE = 0.01f * 0.01f;
    protected static readonly float MAX_BODY_SIZE = 2048f * 2048f;

    protected static readonly float MIN_DENSITY = 0.10f;
    protected static readonly float MAX_DENSITY = 22.5f;

    protected static float gravity = 9.81f;

}

