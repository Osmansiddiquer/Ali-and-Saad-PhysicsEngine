
using PhysicsEngine.src.physics._2D.body;
using System.Numerics;

namespace PhysicsEngine.src.physics._2D.collision;

public readonly struct CollisionManifold
{
    // 2 colliding bodies
    public readonly PhysicsBody2D BODY_A;
    public readonly PhysicsBody2D BODY_B;

    // Collision normal and depth
    public readonly Vector2 NORMAL;
    public readonly float DEPTH;

    // Contact points on collision
    public readonly Vector2 CONTACT_P1;
    public readonly Vector2 CONTACT_P2;

    // Number of contact points (Max 2)
    public readonly int CONTACT_COUNT;

    // Constructor
    public CollisionManifold(PhysicsBody2D bodyA, PhysicsBody2D bodyB, Vector2 normal, float depth, 
        Vector2 contactP1, Vector2 contactP2, int contactCount)
    {
        BODY_A = bodyA;
        BODY_B = bodyB;

        NORMAL = normal;
        DEPTH = depth;

        CONTACT_P1 = contactP1;
        CONTACT_P2 = contactP2;

        CONTACT_COUNT = contactCount;
    }
}