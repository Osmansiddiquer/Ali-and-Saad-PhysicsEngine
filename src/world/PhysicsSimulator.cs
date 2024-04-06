using PhysicsEngine.src.physics._2D.body;
using PhysicsEngine.src.physics._2D.collision;
using Raylib_cs;
using System.Numerics;

namespace PhysicsEngine.src.world;

internal class PhysicsSimulator
{
    // Implement all physics stuff

    private static List<(int, int)> contactPairs = new List<(int, int)>();
    private static List<Vector2> contactPoints = new List<Vector2>();

    internal static void HandlePhysics(List<PhysicsBody2D> bodies)
    {
        contactPoints.Clear();

        for (int it = 0; it < 20; it++)
        {
            HandleCollisions(bodies);
            UpdateBodies(bodies);
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
      
                // Check if objects may be colliding
                if (!CollisionDetection.AABBIntersection(bodyA.GetAABB(), bodyB.GetAABB()))
                    continue;

                contactPairs.Add((i, j));
            }
        }
    }

    private static void CollisionNarrowPhase(List<PhysicsBody2D> bodies)
    {
        // Resolve collision at contact points
        for (int i = 0; i < contactPairs.Count; i++)
        {
            (int, int) pair = contactPairs[i];
            PhysicsBody2D bodyA = bodies[pair.Item1];
            PhysicsBody2D bodyB = bodies[pair.Item2];

            Vector2 normal;
            float depth;

            // Detect collision and add contact points
            if (CollisionDetection.CheckCollision(bodyA, bodyB, out normal, out depth))
            {
                CollisionHelper.FindContactPoints(bodyA, bodyB, out Vector2 contactP1, out Vector2 contactP2, out int contactCount);
                CollisionManifold contact = new CollisionManifold(bodyA, bodyB, normal, depth, contactP1, contactP2, contactCount);
                AddContactPoints(contact, contactPoints);

                CollisionResolution.ResolveCollisionAdvanced(in contact);
                SeperateBodies(bodyA, bodyB, normal * depth);

            }

        }
    }

    private static void SeperateBodies(PhysicsBody2D bodyA, PhysicsBody2D bodyB, Vector2 direction)
    {
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

    // Add contact points for display (DEBUG only)
    private static void AddContactPoints(CollisionManifold contact, List<Vector2> contactPoints)
    {

        if (!contactPoints.Contains(contact.ContactP1))
        {
            contactPoints.Add(contact.ContactP1);

            if (contact.ContactCount > 1 && !contactPoints.Contains(contact.ContactP2))
            {
               contactPoints.Add(contact.ContactP2);
            }

            // Drawing contact points for debugging
            Raylib.DrawRectangle((int)contact.ContactP1.X, (int)contact.ContactP1.Y, 12, 12, Color.Orange);
            Raylib.DrawRectangle((int)contact.ContactP2.X, (int)contact.ContactP2.Y, 12, 12, Color.Orange);
       }
    }

    private static void UpdateCollisionState(PhysicsBody2D body, List<PhysicsBody2D> bodies)
    {
        // Reset all collision-related properties to false initially
        body.IsOnCeiling = false;
        body.IsOnFloor = false;
        body.IsOnWallL = false;
        body.IsOnWallR = false;

        body.Normal = Vector2.Zero;

        // Set collision states based on the current position and velocity of the body
        // You may need to adjust the conditions based on your specific requirements
        foreach (PhysicsBody2D otherBody in bodies)
        {
            if (otherBody != body)
            {
                // Check if there is contact with the other body
                Vector2 normal;
                float depth;

                if (CollisionDetection.CheckCollision(body, otherBody, out normal, out depth))
                {
                    body.IsOnCeiling = normal.Y < -0.5f;
                    body.IsOnFloor = normal.Y > 0.5f;

                    body.IsOnWallL = normal.X < -0.5f;
                    body.IsOnWallR = normal.X > 0.5f;

                    body.Normal = normal;
                }
            }
        }
    }

    private static void UpdateBodies(List<PhysicsBody2D> bodies)
    {
        // Update bodies and collision state
        foreach (PhysicsBody2D body in bodies)
        {
            if (body is RigidBody2D)
                body.RunComponents();

            UpdateCollisionState(body, bodies);
        }
    }
}
