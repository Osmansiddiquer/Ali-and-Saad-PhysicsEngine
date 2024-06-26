﻿using System.Numerics;
using GameEngine.src.physics.component;

namespace GameEngine.src.physics.body;

public class ProjectileBody2D : RigidCircle2D
{
    private List<PhysicsBody2D> bodies;

    internal ProjectileBody2D(Vector2 position, float area, float radius, List<Component> components, 
        Vector2 velocity, List<PhysicsBody2D> bodies, int time) : base(position, 1, 1, area, 1, radius, components)
    {
        // Initialize the projectile
        this.bodies = bodies;
        LinVelocity = velocity;

        // Make the projectile disappear after a certain amount of time
        System.Timers.Timer timer = new System.Timers.Timer(time);
        timer.Elapsed += (sender, e) => onTimeToLive();

        timer.Enabled = true;
    }

    internal ProjectileBody2D(Vector2 position, float area, float radius, List<Component> components, 
        Vector2 velocity, List<PhysicsBody2D> bodies) :
        this(position, area, radius, components, velocity, bodies, 2000) { }

    public override void ProjectileHit (PhysicsBody2D body)
    {
        // Dont do anything if the projectile hits another projectile
        if (body is ProjectileBody2D)
            return;
        
        // Projectile Logic Here


        // Remove the projectile from the list of bodies after it hits something
        bodies.Remove(this);
    }

    private void onTimeToLive()
    {
        bodies.Remove(this);
    }
}


