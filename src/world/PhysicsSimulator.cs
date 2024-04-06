using PhysicsEngine.src.physics._2D.body;
using PhysicsEngine.src.physics._2D.collision;
using Raylib_cs;
using System.Numerics;

namespace PhysicsEngine.src.world;

internal class PhysicsSimulator
{
    private static HashSet<(int, int)> contactPairs = new HashSet<(int, int)>();
    private static Vector2 collisionNormal;

    internal static void HandlePhysics(List<PhysicsBody2D> bodies, double delta)
    {
        for (int it = 0; it < 12; it++)
        {
            HandleCollisions(bodies);
            UpdateBodies(bodies, delta);
        }
    }

    private static void HandleCollisions(List<PhysicsBody2D> bodies)
    {
        contactPairs.Clear();
        CollisionBroadPhase(bodies);
        CollisionNarrowPhase(bodies);
    }

    private static void CollisionBroadPhase(List<PhysicsBody2D> bodies)
    {
        for (int i = 0; i < bodies.Count; i++)
        {
            PhysicsBody2D bodyA = bodies[i];

            for (int j = i + 1; j < bodies.Count; j++)
            {
                PhysicsBody2D bodyB = bodies[j];

                if (CollisionDetection.AABBIntersection(bodyA.GetAABB(), bodyB.GetAABB()))
                    contactPairs.Add((i, j));
            }
        }
    }

    private static void CollisionNarrowPhase(List<PhysicsBody2D> bodies)
    {

        foreach ((int, int) pair in contactPairs)
        {
            PhysicsBody2D bodyA = bodies[pair.Item1];
            PhysicsBody2D bodyB = bodies[pair.Item2];

            Vector2 normal;
            float depth;

            if (CollisionDetection.CheckCollision(bodyA, bodyB, out normal, out depth))
            {
                CollisionHelper.FindContactPoints(bodyA, bodyB, out Vector2 contactP1, out Vector2 contactP2, out int contactCount);
                CollisionManifold contact = new CollisionManifold(bodyA, bodyB, normal, depth, contactP1, contactP2, contactCount);

                collisionNormal = normal;

                CollisionResolution.ResolveCollisionAdvanced(in contact);
                SeparateBodies(bodyA, bodyB, normal * depth);
            }

        }
    }

    private static void SeparateBodies(PhysicsBody2D bodyA, PhysicsBody2D bodyB, Vector2 direction)
    {
        if (bodyA is ProjectileBody2D || bodyB is ProjectileBody2D)
        {
            return;
        }
        if (bodyA is StaticBody2D)  
            bodyB.Translate(direction);

        else if (bodyB is StaticBody2D)
            bodyA.Translate(-direction);

        else
        {
            bodyA.Translate(-direction / 2f);
            bodyB.Translate(direction / 2f);
        }
    }

    private static void UpdateBodies(List<PhysicsBody2D> bodies, double delta)
    {
        foreach (PhysicsBody2D body in bodies)
        {
            if (body is RigidBody2D rigidBody)
                rigidBody.RunComponents(delta);

            body.ResetCollisionState();
            body.UpdateCollisionState(collisionNormal);
        }
    }
}


