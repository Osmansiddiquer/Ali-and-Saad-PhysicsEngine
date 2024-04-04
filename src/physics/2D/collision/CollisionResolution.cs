using PhysicsEngine.src.physics._2D.body;
using Raylib_cs;
using System.Numerics;

namespace PhysicsEngine.src.physics._2D.collision;

public class CollisionResolution
{

    private static List<CollisionManifold> contacts = new List<CollisionManifold>();
    private static List<Vector2> contactPoints = new List<Vector2>();

    // Move objects when they collide
    public static void HandleCollision(List<PhysicsBody2D> bodies)
    {
        float accumulator = 0f;
        float timestep = 1f / 60f;

        contactPoints.Clear();

        // Make sure loop runs only once per frame
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

                    // Check if objects may be colliding
                    if (!CollisionDetection.AABBIntersection(bodyA.GetAABB(), bodyB.GetAABB())) 
                        continue;

                    // Detect collision and add contact points
                    if (CollisionDetection.CheckCollision(bodyA, bodyB, out normal, out depth))
                    {
                        CollisionHelper.FindContactPoints(bodyA, bodyB, out Vector2 contactP1, out Vector2 contactP2, out int contactCount);
                        CollisionManifold contact = new CollisionManifold(bodyA, bodyB, normal, depth, contactP1, contactP2, contactCount);

                        contacts.Add(contact);
                    }
                    else continue;
                }
            }

            // Resolve collision at contact points
            foreach (CollisionManifold contact in contacts)
            {
                ResolveCollision(in contact);

                if (contact.CONTACT_COUNT > 0 && !contactPoints.Contains(contact.CONTACT_P1))
                { 
                    contactPoints.Add(contact.CONTACT_P1);

                    if (contact.CONTACT_COUNT > 1 && !contactPoints.Contains(contact.CONTACT_P2))
                    {
                        contactPoints.Add(contact.CONTACT_P2);
                    }

                    // Drawing contact points for debugging
                    Raylib.DrawRectangle((int)contact.CONTACT_P1.X, (int)contact.CONTACT_P1.Y, 12, 12, Color.Orange);
                }
            }

            // Update collision states after collision resolution
            foreach (PhysicsBody2D body in bodies)
            {
                CollisionHelper.UpdateCollisionState(body, bodies);
            }

            accumulator += timestep;
        }
    }


    // Apply acceleration after collision
    private static void ResolveCollision(in CollisionManifold contact)
    {
        PhysicsBody2D bodyA = contact.BODY_A;
        PhysicsBody2D bodyB = contact.BODY_B;

        Vector2 normal = contact.NORMAL;
        float depth = contact.DEPTH;

        //System.Console.WriteLine(normal);

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

        applyFriction(bodyA, bodyB, normal);
    }

    // Apply friction after collision
    private static void applyFriction(PhysicsBody2D bodyA, PhysicsBody2D bodyB, Vector2 normal)
    {
        // Calculate relative velocity of the two bodies
        Vector2 relativeVelocity = bodyB.LinVelocity - bodyA.LinVelocity;

        // Calculate tangent vector
        Vector2 tangent = new Vector2(-normal.Y, normal.X);

        // Calculate normal force
        float normalForce = Vector2.Dot(relativeVelocity, tangent);

        float frictionCoefficient = bodyA.Substance.StaticFriction * bodyB.Substance.StaticFriction;
        // Apply friction
        Vector2 friction = Vector2.Normalize(tangent) * frictionCoefficient * normalForce;

        bodyA.LinVelocity += friction;
        bodyB.LinVelocity -= friction;

        //System.Console.WriteLine("Friction applied");
    }
}

