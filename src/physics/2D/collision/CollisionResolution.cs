﻿using PhysicsEngine.src.physics._2D.body;
using System.Numerics;

namespace PhysicsEngine.src.physics._2D.collision;

public class CollisionResolution
{
    // Apply acceleration after collision
    public static void ResolveCollision(in CollisionManifold contact)
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

        // Apply impulses to update velocities
        bodyA.LinVelocity -= velBodyA;
        bodyB.LinVelocity += velBodyB;

        // Set linear velocity of bodyA to zero if its magnitude is less than epsilon
        if (bodyA.LinVelocity.Length() < .5f)
            bodyA.LinVelocity = Vector2.Zero;

        // Set linear velocity of bodyB to zero if its magnitude is less than epsilon
        if (bodyB.LinVelocity.Length() < .5f)
            bodyB.LinVelocity = Vector2.Zero;

        // Calculate the direction each body needs to be pushed in
        Vector2 direction = normal * depth * 0.5f;

        // Translate bodies to resolve collision
        bodyA.Translate(-direction);
        bodyB.Translate(direction);

        // Apply friction after collision
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

