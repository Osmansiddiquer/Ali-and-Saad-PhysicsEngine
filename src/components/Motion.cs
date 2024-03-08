using PhysicsEngine.src.body;
using PhysicsEngine.src.physics._2D;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine.src.components;
public class Motion : Component
{
    public override void RunComponent(RigidBody2D body)
    {
        UseMotion(body);
    }

    public void UseMotion(RigidBody2D body)
    {
        body.LinVelocity += body.Force / body.Substance.Mass;

        body.Transform.Translate(body.LinVelocity);
        body.Transform.Rotate(body.RotVelocity);

        body.verticesUpdateRequired = true;

        body.Force = Vector2.Zero;
    }
}

