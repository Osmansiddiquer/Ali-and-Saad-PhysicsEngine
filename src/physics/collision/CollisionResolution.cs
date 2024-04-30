using GameEngine.src.helper;
using GameEngine.src.physics.body;
using System.Numerics;

namespace GameEngine.src.physics.collision;
internal static class CollisionResolution
{
    private static Vector2[] contactList = new Vector2[2];
    
    private static Vector2[] impulseList = new Vector2[2];
    private static float[] jList = new float[2];
    private static Vector2[] frictionImpulseList = new Vector2[2];

    private static Vector2[] rAList = new Vector2[2];
    private static Vector2[] rBList = new Vector2[2];

    internal static void ResolveCollisionBasic(in CollisionManifold contact)
    {
        PhysicsBody2D bodyA = contact.BodyA;
        PhysicsBody2D bodyB = contact.BodyB;

        // If either body is a projectile, handle projectile collision
        if (bodyA is ProjectileBody2D projectileA)
        {
            projectileA.ProjectileHit(bodyB);
            return;
        }
        else if (bodyB is ProjectileBody2D projectileB)
        {
            projectileB.ProjectileHit(bodyA);
            return;
        }

        Vector2 normal = contact.Normal;

        // Calculate relative velocity of the two bodies
        Vector2 relativeVelocity = bodyB.LinVelocity - bodyA.LinVelocity;

        // Calculate restitution of the bodies
        float restitution = MathF.Min(bodyA.Material.Restitution, bodyB.Material.Restitution);

        // Calculate collision impulse for linear motion
        float impulse = -((1 + restitution) * Vector2.Dot(relativeVelocity, normal))
            / ((1f / bodyA.Material.Mass) + (1f / bodyB.Material.Mass));

        // Update velocities after collision for linear motion
        Vector2 accelerationA = impulse / bodyA.Material.Mass * normal;
        Vector2 accelerationB = impulse / bodyB.Material.Mass * normal;

        // Apply impulses to update velocities for linear motion
        bodyA.LinVelocity -= accelerationA;
        bodyB.LinVelocity += accelerationB;
    }

    internal static void ResolveCollisionAdvanced(in CollisionManifold contact)
    {
        // Bodies in contact
        PhysicsBody2D bodyA = contact.BodyA;
        PhysicsBody2D bodyB = contact.BodyB;

        // If either body is a projectile, handle projectile collision
        if (bodyA is ProjectileBody2D projectileA)
        {
            projectileA.ProjectileHit(bodyB);
            return;
        }
        else if (bodyB is ProjectileBody2D projectileB)
        {
            projectileB.ProjectileHit(bodyA);
            return;
        }

        // Collision information
        Vector2 normal = contact.Normal;
        Vector2 contact1 = contact.ContactP1;
        Vector2 contact2 = contact.ContactP2;
        int contactCount = contact.ContactCount;

        // Coefficient of restitution
        float restitution = MathF.Min(bodyA.Material.Restitution, bodyB.Material.Restitution);

        // Store contact points in arrays
        contactList[0] = contact1;
        contactList[1] = contact2;

        // Initialize arrays 
        for (int i = 0; i < contactCount; i++)
        {
            impulseList[i] = Vector2.Zero;
            rAList[i] = Vector2.Zero;
            rBList[i] = Vector2.Zero;
            frictionImpulseList[i] = Vector2.Zero;
            jList[i] = 0f;
        }

        // Calculate impulses 
        for (int i = 0; i < contactCount; i++)
        {
            // Vectors pointing from center to contact point
            Vector2 rA = contactList[i] - bodyA.Transform.Translation;
            Vector2 rB = contactList[i] - bodyB.Transform.Translation;

            rAList[i] = rA;
            rBList[i] = rB;

            // Velocity induced by angular motion at contact points
            Vector2 normalRA = new Vector2(-rA.Y, rA.X);
            Vector2 normalRB = new Vector2(-rB.Y, rB.X);

            Vector2 rotLinVelA = normalRA * MathExtra.Deg2Rad(bodyA.RotVelocity);
            Vector2 rotLinVelB = normalRB * MathExtra.Deg2Rad(bodyB.RotVelocity);

            // Relative velocity
            Vector2 relativeVelocity =
                (bodyB.LinVelocity + rotLinVelB) -
                (bodyA.LinVelocity + rotLinVelA);

            // If objects are separating, skip this contact point
            if (Vector2.Dot(relativeVelocity, normal) > 0f)
            {
                continue;
            }

            // Calculate normal and tangential components of relative positions
            float normalRADotN = Vector2.Dot(normalRA, normal);
            float normalRBDotN = Vector2.Dot(normalRB, normal);

            // Calculate denominator for impulse calculation
            float denom = 1f / bodyA.Material.Mass + 1f / bodyB.Material.Mass +
                (normalRADotN * normalRADotN) / bodyA.MomentOfInertia +
                (normalRBDotN * normalRBDotN) / bodyB.MomentOfInertia;

            // Magnitude of impulse
            float j = -(1f + restitution) * Vector2.Dot(relativeVelocity, normal);
            j /= denom;
            j /= contactCount;

            // Store impulse
            jList[i] = j;
            impulseList[i] = j * normal;
        }

        // Apply linear and rotational impulse
        for (int i = 0; i < contactCount; i++)
        {
            Vector2 impulse = impulseList[i];
            Vector2 ra = rAList[i];
            Vector2 rb = rBList[i];

            bodyA.LinVelocity -= impulse / bodyA.Material.Mass;
            bodyA.RotVelocity -= MathExtra.Cross(ra, impulse) / bodyA.MomentOfInertia * (180f / MathF.PI);

            bodyB.LinVelocity += impulse / bodyB.Material.Mass;
            bodyB.RotVelocity += MathExtra.Cross(rb, impulse) / bodyB.MomentOfInertia * (180f / MathF.PI);
        }

        // Calculate friction impulses
        for (int i = 0; i < contactCount; i++)
        {
            // Calculate relative positions of contact points
            Vector2 rA = contactList[i] - bodyA.Transform.Translation;
            Vector2 rB = contactList[i] - bodyB.Transform.Translation;


            // Velocity induced by angular motion at contact points
            Vector2 normalRA = new Vector2(-rA.Y, rA.X);
            Vector2 normalRB = new Vector2(-rB.Y, rB.X);
            Vector2 rotLinVelA = normalRA * bodyA.RotVelocity * MathF.PI / 180;
            Vector2 rotLinVelB = normalRB * bodyB.RotVelocity * MathF.PI / 180;

            // Relative velocity
            Vector2 relativeVelocity =
                (bodyB.LinVelocity + rotLinVelB) -
                (bodyA.LinVelocity + rotLinVelA);

            // Calculate tangent direction
            Vector2 tangent = relativeVelocity - Vector2.Dot(relativeVelocity, normal) * normal;

            // If relative velocity is very small, skip this contact point
            if (tangent.Length() < 0.0005f)
            {
                continue;
            }
            else
            {
                tangent = Vector2.Normalize(tangent);
            }

            // Calculate tangential components of relative positions
            float normalRADotT = Vector2.Dot(normalRA, tangent);
            float normalRBDotT = Vector2.Dot(normalRB, tangent);

            // Calculate denominator for friction impulse calculation
            float denom = 1f / bodyA.Material.Mass + 1f / bodyB.Material.Mass +
                (normalRADotT * normalRADotT) * 1f / bodyA.MomentOfInertia +
                (normalRBDotT * normalRBDotT) * 1f / bodyB.MomentOfInertia;

            // Calculate friction impulse magnitude
            float jt = -Vector2.Dot(relativeVelocity, tangent);
            jt /= denom;
            jt /= contactCount;

            // Determine whether static or dynamic friction should be used
            Vector2 frictionImpulse;
            float j = jList[i];

            if (MathF.Abs(jt) <= j * (bodyA.Material.StaticFriction + bodyB.Material.StaticFriction) / 2)
            {
                frictionImpulse = jt * tangent;
            }
            else
            {
                frictionImpulse = -j * tangent * (bodyA.Material.DynamicFriction + bodyB.Material.DynamicFriction) / 2;
            }

            // Store friction impulse
            frictionImpulseList[i] = frictionImpulse;
        }

        // Apply linear and rotational friction impulses
        for (int i = 0; i < contactCount; i++)
        {
            Vector2 frictionImpulse = frictionImpulseList[i];
            Vector2 ra = rAList[i];
            Vector2 rb = rBList[i];

            bodyA.LinVelocity -= frictionImpulse / bodyA.Material.Mass;
            bodyA.RotVelocity -= MathExtra.Cross(ra, frictionImpulse) / bodyA.MomentOfInertia * (180f / MathF.PI);

            bodyB.LinVelocity += frictionImpulse / bodyB.Material.Mass;
            bodyB.RotVelocity += MathExtra.Cross(rb, frictionImpulse) / bodyB.MomentOfInertia * (180f / MathF.PI);
        }
    }

}

