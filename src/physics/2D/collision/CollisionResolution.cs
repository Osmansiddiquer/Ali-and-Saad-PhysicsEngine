using PhysicsEngine.src.body;
using PhysicsEngine.src.physics._2D.collision;
using Raylib_cs;
using System.Numerics;

namespace PhysicsEngine.src.physics._2D;

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
            for (int i = 0; i < bodies.Count; i++)
            {
                PhysicsBody2D bodyA = bodies[i];
                for (int j = i + 1; j < bodies.Count; j++)
                {
                    PhysicsBody2D bodyB = bodies[j];

                    // Resolve collision 
                    if (CollisionDetection.CheckCollision(bodyA, bodyB, out Vector2 normal, out float depth))
                    {
                        CollisionManifold contact = new CollisionManifold(bodyA, bodyB, normal, depth, Vector2.Zero, Vector2.Zero, 0);
                        contacts.Add(contact);
                    }
                    else continue;
                }
            }

            foreach (CollisionManifold contact in contacts)
            {
                ResolveCollision(in contact);
            }
            accumulator += timestep;
        }


    }

    private static void ResolveCollision(in CollisionManifold contact)
    {
        // Relative velocity of the 2 bodies
        Vector2 velocity = contact.BodyB.LinVelocity - contact.BodyA.LinVelocity;

        // Restitution of bodies
        float restitution = MathF.Min(contact.BodyA.Substance.Restitution, contact.BodyB.Substance.Restitution);

        // Impulse of collisions
        float impulse = -((1 + restitution) * Vector2.Dot(velocity, contact.Normal)) 
            / ((1f / contact.BodyA.Substance.Mass) + (1f / contact.BodyB.Substance.Mass));

        // Calculate velocity after collision
        contact.BodyA.LinVelocity -= impulse / contact.BodyA.Substance.Mass * contact.Normal;
        contact.BodyB.LinVelocity += impulse / contact.BodyB.Substance.Mass * contact.Normal;

        // Calculate the direction each body needs to be pushed in
        Vector2 direction = contact.Normal * contact.Depth * 0.5f;

        // Adjust direction for circle and circle collisions
        direction *=
            (contact.BodyA.Shape is ShapeType.Circle && contact.BodyB.Shape is ShapeType.Circle ||
            contact.BodyA.Shape is ShapeType.Box && contact.BodyB.Shape is ShapeType.Circle) ? -1f : 1f;

        contact.BodyA.Translate(-direction);
        contact.BodyB.Translate(direction);
    }
}