//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Numerics;
//using System.Text;
//using System.Threading.Tasks;

//using PhysicsEngine.src.physics._2D;
//namespace PhysicsEngine.src.body;

//public class ProjectileBody2D : RigidBody2D
//{

//    public ProjectileBody2D(Vector2 position, float rotation, Vector2 scale, float mass, float density, float area, float restitution, float radius, float width, float height, ShapeType shape)
//    {
//        Transform = new physics._2D.Transform2D(position, rotation, scale);
//        Dimensions = new Dimensions2D(radius, width, height);
//        Substance = new Substance2D(mass, density, area, restitution, false);

//        Shape = ShapeType.Circle;

//        LinVelocity = Vector2.Zero;
//        RotVelocity = 0f;

//        Force = Vector2.Zero;

//        // No vertices for circle
//        vertices = null;
//        transformedVertices = null;

//        Triangles = null;

//        verticesUpdateRequired = true;
//    }
//}