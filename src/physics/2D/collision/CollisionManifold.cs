
using PhysicsEngine.src.body;
using System.Numerics;

namespace PhysicsEngine.src.physics._2D.collision;

public readonly struct CollisionManifold
{
    public readonly PhysicsBody2D BodyA;
    public readonly PhysicsBody2D BodyB;

    public readonly Vector2 Normal;
    public readonly float Depth;

    public readonly Vector2 ContactP1;
    public readonly Vector2 ContactP2;

    public readonly int ContactCount;

    public CollisionManifold(PhysicsBody2D bodyA, PhysicsBody2D bodyB, Vector2 normal, float depth, 
        Vector2 contactP1, Vector2 contactP2, int contactCount)
    {
        BodyA = bodyA;
        BodyB = bodyB;

        Normal = normal;
        Depth = depth;

        ContactP1 = contactP1;
        ContactP2 = contactP2;

        ContactCount = contactCount;
    }
}