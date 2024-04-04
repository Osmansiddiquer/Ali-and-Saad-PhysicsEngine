using System.Collections.Generic;
using System.Numerics;
using PhysicsEngine.res.scenes;
using PhysicsEngine.src.components;



namespace PhysicsEngine.src.physics._2D.body;

public class ProjectileBody2D : RigidCircle2D
{
    List<PhysicsBody2D> bodies;
    public ProjectileBody2D(Vector2 position, Vector2 scale, float mass, float density, float area, float restitution,
        float radius, List<Component> components, Vector2 velocity, List<PhysicsBody2D> bodies) : base(position, scale, mass,density, area, restitution, radius, components)
    {
        this.bodies = bodies;
        this.LinVelocity = velocity;
        this.Name = "Projectile";
    }

    public void ProjectileHit (PhysicsBody2D body)
    {
        if (body.Name == "Projectile")
        {
            return;
        }
        bodies.Remove(this);
    }

}


