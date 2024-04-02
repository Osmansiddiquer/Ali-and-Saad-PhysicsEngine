using PhysicsEngine.src.physics._2D.body;
using System.Numerics;

namespace PhysicsEngine.src.physics._2D.collision;

public class CollisionResolution
{

    private static List<CollisionManifold> contacts = new List<CollisionManifold>();

    // Move objects when they collide
    public static void HandleCollision(List<PhysicsBody2D> bodies)
    {
        float accumulator = 0f;
        float timestep = 1f/60f;

        // Check collisions only once per frame
        while (accumulator < timestep)
        {
            contacts.Clear();
            // Iterate through list of bodies to check for collision
            for (int i = 0; i < bodies.Count; i++)
            {
                // Take a body and check it's collision with every other body
                PhysicsBody2D bodyA = bodies[i];

                for (int j = i + 1; j < bodies.Count; j++)
                {
                    PhysicsBody2D bodyB = bodies[j];

                    // Get contact points on collision (yet to be implemented)
                    if (CollisionDetection.CheckCollision(bodyA, bodyB, out Vector2 normal, out float depth))
                    {
                        CollisionManifold contact = new CollisionManifold(bodyA, bodyB, normal, depth, Vector2.Zero, Vector2.Zero, 0);
                        contacts.Add(contact);

                        SetCollisionState(bodyA, bodyB, normal);
                    }

                    else
                    {

                        bodyA.IsOnFloor = bodyB.IsOnFloor = bodyA.IsOnCeiling = bodyB.IsOnCeiling = 
                            bodyA.IsOnWallR = bodyB.IsOnWallR = bodyA.IsOnWallL = bodyB.IsOnWallL = false;
                        continue;
                    }
                }
            }

            // Resolve collision on each contact point
            foreach (CollisionManifold contact in contacts)
            {
                ResolveCollision(in contact);
            }

            accumulator += timestep;
        }
    }

    private static void ResolveCollision(in CollisionManifold contact)
    {
        PhysicsBody2D bodyA = contact.BODY_A;
        PhysicsBody2D bodyB = contact.BODY_B;

        Vector2 normal = contact.NORMAL;
        float depth = contact.DEPTH;

        // Calculate relative velocity of the two bodies
        Vector2 relativeVelocity = bodyB.LinVelocity - bodyA.LinVelocity;

        // Calculate restitution of the bodies
        float restitution = MathF.Min(bodyA.Substance.Restitution, bodyB.Substance.Restitution);

        // Calculate collision impulse
        float impulse = -((1 + restitution) * Vector2.Dot(relativeVelocity, normal))
            / ((1f / bodyA.Substance.Mass) + (1f / bodyB.Substance.Mass));

        // Update velocities after collision
        Vector2 velBodyA = impulse / bodyA.Substance.Mass * normal;
        Vector2 velBodyB = impulse / bodyB.Substance.Mass * normal;

        bodyA.LinVelocity -= velBodyA;
        bodyB.LinVelocity += velBodyB;

        // Calculate the direction each body needs to be pushed in
        Vector2 direction = normal * depth * 0.5f;

        // Translate bodies to resolve collision
        bodyA.Translate(-direction);
        bodyB.Translate(direction);
    }

    private static void SetCollisionState(PhysicsBody2D bodyA, PhysicsBody2D bodyB, Vector2 normal)
    {
        bodyA.IsOnCeiling = normal.Y >= 0;
        bodyB.IsOnFloor = normal.Y >= 0;

        bodyA.IsOnFloor = normal.Y <= 0;
        bodyB.IsOnCeiling = normal.Y <= 0;

        bodyA.IsOnWallL = normal.X >= 0;
        bodyB.IsOnWallR = normal.X >= 0;

        bodyA.IsOnWallR = normal.X <= 0;
        bodyB.IsOnWallL = normal.X <= 0;
    }
}