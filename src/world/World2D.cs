using System;

namespace PhysicsEngine.src.world;
public class World2D
{
    // Constraints

    public static readonly float MIN_BODY_SIZE = 0.01f * 0.01f;
    public static readonly float MAX_BODY_SIZE = 2048f * 2048f;

    public static readonly float MIN_DENSITY = 0.10f;
    public static readonly float MAX_DENSITY = 22.5f;
}

