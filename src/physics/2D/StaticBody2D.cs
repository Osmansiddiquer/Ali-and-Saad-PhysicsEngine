using PhysicsEngine.src.physics;
using System;
using System.Numerics;

namespace PhysicsEngine.src.body;

public sealed class StaticBody2D : PhysicsBody2D
{

    public readonly float Area;

    // Constructor
    public StaticBody2D(Vector2 Position, float Rotation, Vector2 Scale, 
        float Area, float Radius, float Width, float Height, ShapeType Shape) 
    {
        this.Position = Position;
        this.Rotation = Rotation;
        this.Scale = Scale;

        this.Shape = Shape;

        this.Area = Area;
        this.Radius = Radius;
        this.Width = Width;
        this.Height = Height;
    }
}

