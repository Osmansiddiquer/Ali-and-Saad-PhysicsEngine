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
        float timestep = 1f / 60f;

        while (accumulator < timestep)
        {
            contacts.Clear();

            for (int i = 0; i < bodies.Count; i++)
            {
                PhysicsBody2D bodyA = bodies[i];

                for (int j = i + 1; j < bodies.Count; j++)
                {
                    PhysicsBody2D bodyB = bodies[j];
                    Vector2 normal = Vector2.Zero;
                    float depth = 0f;

                    if (CollisionDetection.CheckCollision(bodyA, bodyB, out normal, out depth))
                    {
                        CollisionManifold contact = new CollisionManifold(bodyA, bodyB, normal, depth, Vector2.Zero, Vector2.Zero, 0);
                        contacts.Add(contact);
                    }
                }
            }

            foreach (CollisionManifold contact in contacts)
            {
                ResolveCollision(in contact);
            }

            // Update collision states after collision resolution
            foreach (PhysicsBody2D body in bodies)
            {
                UpdateCollisionState(body, bodies);
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

    private static void UpdateCollisionState(PhysicsBody2D body, List<PhysicsBody2D> allBodies)
    {
        // Reset all collision-related properties to false initially
        body.IsOnCeiling = false;
        body.IsOnFloor = false;
        body.IsOnWallL = false;
        body.IsOnWallR = false;

        // Set collision states based on the current position and velocity of the body
        // You may need to adjust the conditions based on your specific requirements
        foreach (PhysicsBody2D otherBody in allBodies)
        {
            if (otherBody != body)
            {
                // Check if there is contact with the other body
                Vector2 normal;
                float depth;
                if (CollisionDetection.CheckCollision(body, otherBody, out normal, out depth))
                {
                    if (normal.Y < 0)
                        body.IsOnCeiling = true;
                    else if (normal.Y > 0)
                        body.IsOnFloor = true;

                    if (normal.X > 0)
                        body.IsOnWallL = true;
                    else if (normal.X < 0)
                        body.IsOnWallR = true;
                }
            }
        }
    }
}