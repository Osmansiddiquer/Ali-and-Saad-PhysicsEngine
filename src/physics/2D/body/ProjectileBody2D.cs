using System.Collections.Generic;
using System.Numerics;
using PhysicsEngine.res.scenes;
using PhysicsEngine.src.components;
using System.Timers;

namespace PhysicsEngine.src.physics._2D.body;

public class ProjectileBody2D : RigidCircle2D
{
    private List<PhysicsBody2D> bodies;

    public ProjectileBody2D(Vector2 position, Vector2 scale, float mass, float density, float area, float restitution,
        float radius, List<Component> components, Vector2 velocity, List<PhysicsBody2D> bodies, int time) : base(position, scale, mass,density, area, restitution, radius, components)
    {
        this.bodies = bodies;
        this.LinVelocity = velocity;
        this.Name = "Projectile";

        System.Timers.Timer timer = new System.Timers.Timer(time);
        timer.Elapsed += (sender, e) => onTimeToLive();

        timer.Enabled = true;
    }

    public ProjectileBody2D(Vector2 position, Vector2 scale, float mass, float density, float area, float restitution,
        float radius, List<Component> components, Vector2 velocity, List<PhysicsBody2D> bodies) :
        this(position, scale, mass, density, area, restitution, radius, components, velocity, bodies, 1000) { }

    public void ProjectileHit (PhysicsBody2D body)
    {
        if (body.Name == "Projectile")
        {
            return;
        }
        bodies.Remove(this);
    }

    private void onTimeToLive()
    {
        bodies.Remove(this);
    }
}


