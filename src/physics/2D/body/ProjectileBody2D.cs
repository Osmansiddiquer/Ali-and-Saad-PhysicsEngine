using System.Numerics;
using PhysicsEngine.src.components;

namespace PhysicsEngine.src.physics._2D.body;

public class ProjectileBody2D : RigidCircle2D
{
    private List<PhysicsBody2D> bodies;

    internal ProjectileBody2D(Vector2 position, Vector2 scale, float mass, float density, float area, float restitution,
        float radius, List<Component> components, Vector2 force, List<PhysicsBody2D> bodies, int time) : base(position, scale, mass,density, area, restitution, radius, components)
    {
        // Initialize the projectile
        this.bodies = bodies;
        Force = force;

        // Make the projectile disappear after a certain amount of time
        System.Timers.Timer timer = new System.Timers.Timer(time);
        timer.Elapsed += (sender, e) => onTimeToLive();

        timer.Enabled = true;
    }

    internal ProjectileBody2D(Vector2 position, Vector2 scale, float mass, float density, float area, float restitution,
        float radius, List<Component> components, Vector2 velocity, List<PhysicsBody2D> bodies) :
        this(position, scale, mass, density, area, restitution, radius, components, velocity, bodies, 1000) { }

    public void ProjectileHit (PhysicsBody2D body)
    {
        // Dont do anything if the projectile hits another projectile
        if (body.Name == "Projectile")
        {
            return;
        }
        // Projectile Logic Here


        // Remove the projectile from the list of bodies after it hits something
        bodies.Remove(this);
    }

    private void onTimeToLive()
    {
        bodies.Remove(this);
    }
}


