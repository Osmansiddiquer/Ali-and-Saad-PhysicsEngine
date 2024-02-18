using PhysicsEngine.src.world;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine.src.physics;

// Types of Shapes
public enum ShapeType {
    Circle, Box
}

public class CollisionBody2D : World2D
{
    // World Transofrm
    public Vector2 Position;
    public Vector2 Scale;
    public float Rotation;

    public ShapeType Shape;

    // Shape Type: Circle
    public float Radius;

    // Shape Type: Box
    public float Width;
    public float Height;

}

