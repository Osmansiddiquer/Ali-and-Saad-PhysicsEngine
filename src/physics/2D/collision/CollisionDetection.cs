using PhysicsEngine.src.physics._2D.body;
using System.Numerics;
using System.Runtime.ConstrainedExecution;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.

namespace PhysicsEngine.src.physics._2D.collision;

public static class CollisionDetection
{

    /* Collisions using seperating axis theorem */
    private static bool CircPolyCollision(PhysicsBody2D bodyA, PhysicsBody2D bodyB, out Vector2 normal, out float depth)
    {
        // Initializing variables for calculation
        normal = Vector2.Zero;
        depth = float.MaxValue;

        Vector2 centerC = Vector2.Zero;
        float radius = 0;

        Vector2 centerP = Vector2.Zero;
        Vector2[] vertices = null;

        Vector2 centerA = Vector2.Zero;
        Vector2 centerB = Vector2.Zero;

        // Get vertices, radius and centers for shapes
        if (bodyA.Shape == bodyB.Shape) return false;
        else
        {
            if (bodyA.Shape == ShapeType.Circle)
            {
                centerC = bodyA.Transform.Position;
                radius = bodyA.Dimensions.Radius;

                centerP = bodyB.Transform.Position;
                vertices = bodyB.GetTransformedVertices();

                centerA = centerC;
                centerB = centerP;
            }

            else
            {
                centerC = bodyB.Transform.Position; 
                radius = bodyB.Dimensions.Radius;

                centerP = bodyA.Transform.Position;
                vertices = bodyA.GetTransformedVertices();

                centerA = centerP;
                centerB = centerC;
            }
        }

        Vector2 axis = Vector2.Zero;
        float axisDepth = 0f;

        float minA, maxA, minB, maxB = 0f;

        // Polygon  
        for (int i = 0; i < vertices.Length; i++)
        {
            // Get a pair of vertices from polygon 
            Vector2 vertexA = vertices[i];
            Vector2 vertexB = vertices[(i + 1) % vertices.Length];

            // Get an edge and it's perpendicular axis
            Vector2 edge = vertexB - vertexA;
            axis = new Vector2(-edge.Y, edge.X);
            axis = Vector2.Normalize(axis);

            // Project vertices to the normal axis
            CollisionHelper.ProjectVertices(vertices, axis, out minA, out maxA);
            CollisionHelper.ProjectCircle(centerC, radius, axis, out minB, out maxB);

            // No collision
            if (minA >= maxB || minB >= maxA)
                return false;

            axisDepth = MathF.Min(maxB - minA, maxA - minB);

            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }

        }

        // Get closest vertex to circle center
        Vector2 closestVertex = vertices[CollisionHelper.FindClosestPolygonVertex(centerC, vertices)];
        axis = closestVertex - centerC;
        axis = Vector2.Normalize(axis);

        // Project vertices to the normal axis
        CollisionHelper.ProjectVertices(vertices, axis, out minA, out maxA);
        CollisionHelper.ProjectCircle(centerC, radius, axis, out minB, out maxB);

        // No collision
        if (minA >= maxB || minB >= maxA)
            return false;

        axisDepth = MathF.Min(maxB - minA, maxA - minB);

        if (axisDepth < depth)
        {
            depth = axisDepth;
            normal = axis;
        }

        Vector2 direction;

        // Direction from polygon to circle center
        direction = centerB - centerA;

        // Correct normal based on direction
        normal = Vector2.Dot(direction, normal) < 0f ? -normal : normal;
        Console.WriteLine(normal);

        // Collision detected
        return true;
    }

    private static bool PolygonCollisions(PhysicsBody2D bodyA, PhysicsBody2D bodyB, out Vector2 normal, out float depth)
    {
        // Collision normal and depth
        normal = Vector2.Zero;
        depth = float.MaxValue;

        if (bodyA.Shape == ShapeType.Circle || bodyB.Shape == ShapeType.Circle) return false;

        // Get vertices and centers for shapes
        Vector2 centerA = bodyA.Transform.Position;
        Vector2[] verticesA = bodyA.GetTransformedVertices();

        Vector2 centerB = bodyB.Transform.Position;
        Vector2[] verticesB = bodyB.GetTransformedVertices();

        // Polygon 1 
        for (int i = 0; i < verticesA.Length; i++)
        {
            // Get a pair of vertices from polygon 1
            Vector2 vertexA = verticesA[i];
            Vector2 vertexB = verticesA[(i + 1) % verticesA.Length];

            // Get an edge and it's perpendicular axis
            Vector2 edge = vertexB - vertexA;
            Vector2 axis = new Vector2(-edge.Y, edge.X);
            axis = Vector2.Normalize(axis);

            // Project vertices to the normal axis
            CollisionHelper.ProjectVertices(verticesA, axis, out float minA, out float maxA);
            CollisionHelper.ProjectVertices(verticesB, axis, out float minB, out float maxB);

            // No collision
            if (minA >= maxB || minB >= maxA) 
                return false;

            float axisDepth = MathF.Min(maxB - minA, maxA - minB);
            
            if (axisDepth < depth) {
                depth = axisDepth;
                normal = axis;
            }
        }

        
        // Polygon 2
        for (int i = 0; i < verticesB.Length; i++)
        {
            // Get a pair of vertices from polygon 2
            Vector2 vertexA = verticesB[i];
            Vector2 vertexB = verticesB[(i + 1) % verticesB.Length];

            // Get the edge and it's perpendicular axis
            Vector2 edge = vertexB - vertexA;
            Vector2 axis = new Vector2(-edge.Y, edge.X);

            // Project vertices to the normal axis
            CollisionHelper.ProjectVertices(verticesA, axis, out float minA, out float maxA);
            CollisionHelper.ProjectVertices(verticesB, axis, out float minB, out float maxB);
            axis = Vector2.Normalize(axis);

            // No collision
            if (minA >= maxB || minB >= maxA)  
                return false;

            float axisDepth = MathF.Min(maxB - minA, maxA - minB);

            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }
        }

        // Direction from centerB to centerA
        Vector2 direction = centerB - centerA;

        // Correct normal based on direction
        normal = Vector2.Dot(direction, normal) < 0f ? -normal : normal;
        Console.WriteLine(normal);

        // Collision
        return true;
    }

    private static bool CicrcleCollisions(PhysicsBody2D bodyA, PhysicsBody2D bodyB, out Vector2 normal, out float depth)
    {
        normal = Vector2.Zero;
        depth = 0f;

        if (bodyA.Shape != ShapeType.Circle || bodyB.Shape != ShapeType.Circle) return false;

        // Get radii and center for shapes
        Vector2 centerA = bodyA.Transform.Position;
        float radiusA = bodyA.Dimensions.Radius;
        
        Vector2 centerB = bodyB.Transform.Position;
        float radiusB = bodyB.Dimensions.Radius;


        // Get distance between the circles and sum of radii
        float distance = Vector2.Distance(centerA, centerB);
        float totalRadii = radiusA + radiusB;

        // Get normal and depth of collision
        normal = Vector2.Normalize(centerB - centerA);
        depth = totalRadii - distance;

        Console.WriteLine(normal);

        // Return true if collision occured
        return distance < totalRadii ? true : false;
    }

    // Check if any collision occurred
    public static bool CheckCollision(PhysicsBody2D bodyA, PhysicsBody2D bodyB, out Vector2 normal, out float depth)
    {
        return CircPolyCollision(bodyA, bodyB, out normal, out depth) 
            || PolygonCollisions(bodyA, bodyB, out normal, out depth) || CicrcleCollisions(bodyA, bodyB, out normal, out depth);

    }
}

