using PhysicsEngine.src.physics._2D.body;
using Raylib_cs;
using System.Numerics;

namespace PhysicsEngine.src.physics._2D.collision;

public class CollisionResolution
{

    private static List<CollisionManifold> contacts = new List<CollisionManifold>();

    // Move objects when they collide
    public static void HandleCollision(List<PhysicsBody2D> bodies)
    {
        float accumulator = 0f;
        float timestep = Raylib.GetFrameTime();

        
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
                    }
                    else continue;
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
        bodyA.LinVelocity -= impulse / bodyA.Substance.Mass * normal;
        bodyB.LinVelocity += impulse / bodyB.Substance.Mass * normal;

        // Calculate the direction each body needs to be pushed in
        Vector2 direction = normal * depth * 0.5f;

        // Adjust direction for specific collision types
        if ((bodyA.Shape == ShapeType.Circle && bodyB.Shape == ShapeType.Circle) ||
            (bodyA.Shape == ShapeType.Box && bodyB.Shape == ShapeType.Circle))   
            direction *= -1f;
        

        // Translate bodies to resolve collision
        bodyA.Translate(-direction);
        bodyB.Translate(direction);
    }
}