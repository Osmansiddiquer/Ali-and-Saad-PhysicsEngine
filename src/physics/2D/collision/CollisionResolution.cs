using PhysicsEngine.src.physics._2D.body;
using System.Numerics;

namespace PhysicsEngine.src.physics._2D.collision;
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
        float restitution = MathF.Min(bodyA.Substance.Restitution, bodyB.Substance.Restitution);

        // Calculate collision impulse for linear motion
        float impulse = -((1 + restitution) * Vector2.Dot(relativeVelocity, normal))
            / ((1f / bodyA.Substance.Mass) + (1f / bodyB.Substance.Mass));

        // Update velocities after collision for linear motion
        Vector2 accelerationA = impulse / bodyA.Substance.Mass * normal;
        Vector2 accelerationB = impulse / bodyB.Substance.Mass * normal;

        // Apply impulses to update velocities for linear motion
        bodyA.LinVelocity -= accelerationA;
        bodyB.LinVelocity += accelerationB;
    }

    internal static void ResolveCollisionAdvanced(in CollisionManifold contact)
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

        Vector2 contact1 = contact.ContactP1;
        Vector2 contact2 = contact.ContactP2;
        int contactCount = contact.ContactCount;

        float restitution = MathF.Min(bodyA.Substance.Restitution, bodyB.Substance.Restitution);

        contactList[0] = contact1;
        contactList[1] = contact2;

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
            Vector2 rA = contactList[i] - bodyA.Transform.Position;
            Vector2 rB = contactList[i] - bodyB.Transform.Position;

            rAList[i] = rA;
            rBList[i] = rB;

            Vector2 normalRA = new Vector2(-rA.Y, rA.X);
            Vector2 normalRB = new Vector2(-rB.Y, rB.X);

            Vector2 rotLinVelA = normalRA * bodyA.RotVelocity * MathF.PI / 180;
            Vector2 rotLinVelB = normalRB * bodyB.RotVelocity * MathF.PI / 180;

            Vector2 relativeVelocity =
                (bodyB.LinVelocity + rotLinVelB) -
                (bodyA.LinVelocity + rotLinVelA);

            float contactVelocityMag = Vector2.Dot(relativeVelocity, normal);

            if (contactVelocityMag > 0f)
            {
                continue;
            }

            float raPerpDotN = Vector2.Dot(normalRA, normal);
            float rbPerpDotN = Vector2.Dot(normalRB, normal);

            float denom = 1f / bodyA.Substance.Mass + 1f / bodyB.Substance.Mass +
                (raPerpDotN * raPerpDotN) / bodyA.MomentOfInertia +
                (rbPerpDotN * rbPerpDotN) / bodyB.MomentOfInertia;

            float j = -(1f + restitution) * contactVelocityMag;
            j /= denom;
            j /= contactCount;

            jList[i] = j;

            Vector2 impulse = j * normal;
            impulseList[i] = impulse;
        }

        // Apply impulses
        for (int i = 0; i < contactCount; i++)
        {
            Vector2 impulse = impulseList[i];
            Vector2 ra = rAList[i];
            Vector2 rb = rBList[i];

            bodyA.LinVelocity -= impulse / bodyA.Substance.Mass;
            bodyA.RotVelocity -= (Cross(ra, impulse) / bodyA.MomentOfInertia) * (180f / MathF.PI);

            bodyB.LinVelocity += impulse / bodyB.Substance.Mass;
            bodyB.RotVelocity += (Cross(rb, impulse) / bodyB.MomentOfInertia) * (180f / MathF.PI);
        }

        for (int i = 0; i < contactCount; i++)
        {
            Vector2 rA = contactList[i] - bodyA.Transform.Position;
            Vector2 rB = contactList[i] - bodyB.Transform.Position;

            rAList[i] = rA;
            rBList[i] = rB;

            Vector2 raPerp = new Vector2(-rA.Y, rA.X);
            Vector2 rbPerp = new Vector2(-rB.Y, rB.X);

            Vector2 angularLinearVelocityA = raPerp * bodyA.RotVelocity * MathF.PI / 180;
            Vector2 angularLinearVelocityB = rbPerp * bodyB.RotVelocity * MathF.PI / 180;

            Vector2 relativeVelocity =
                (bodyB.LinVelocity + angularLinearVelocityB) -
                (bodyA.LinVelocity + angularLinearVelocityA);

            Vector2 tangent = relativeVelocity - Vector2.Dot(relativeVelocity, normal) * normal;

            if (tangent.Length() < 0.0005f)
            {
                continue;
            }
            else
            {
                tangent = Vector2.Normalize(tangent);
            }

            float raPerpDotT = Vector2.Dot(raPerp, tangent);
            float rbPerpDotT = Vector2.Dot(rbPerp, tangent);

            float denom = 1f / bodyA.Substance.Mass + 1f / bodyB.Substance.Mass +
                (raPerpDotT * raPerpDotT) * 1f / bodyA.MomentOfInertia +
                (rbPerpDotT * rbPerpDotT) * 1f / bodyB.MomentOfInertia;

            float jt = -Vector2.Dot(relativeVelocity, tangent);
            jt /= denom;
            jt /= contactCount;

            Vector2 frictionImpulse;
            float j = jList[i];

            if (MathF.Abs(jt) <= j * (bodyA.Substance.StaticFriction + bodyB.Substance.StaticFriction) / 2)
            {
                frictionImpulse = jt * tangent;
            }
            else
            {
                frictionImpulse = -j * tangent * (bodyA.Substance.DynamicFriction + bodyB.Substance.DynamicFriction) / 2;
            }

            frictionImpulseList[i] = frictionImpulse;
        }

        for (int i = 0; i < contactCount; i++)
        {
            Vector2 frictionImpulse = frictionImpulseList[i];
            Vector2 ra = rAList[i];
            Vector2 rb = rBList[i];

            bodyA.LinVelocity -= frictionImpulse / bodyA.Substance.Mass;
            bodyA.RotVelocity -= Cross(ra, frictionImpulse) / bodyA.MomentOfInertia * (180f / MathF.PI);

            bodyB.LinVelocity += frictionImpulse / bodyB.Substance.Mass;
            bodyB.RotVelocity += (Cross(rb, frictionImpulse) / bodyB.MomentOfInertia) * (180f / MathF.PI);
        }
    }

    public static float Cross(Vector2 a, Vector2 b)
    {
        // cz = ax * by − ay * bx
        return a.X * b.Y - a.Y * b.X;
    }

}

