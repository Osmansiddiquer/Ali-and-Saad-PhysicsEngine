using PhysicsEngine.src.physics._2D.body;
using System.Numerics;

namespace PhysicsEngine.src.physics._2D.collision;

public static class CollisionHelper
{
    public static void ProjectCircle(Vector2 centerC, float radius, Vector2 axis, out float min, out float max)
    {
        /*
        Draw a diameter line perpendicular
        to the axis and then get the points
        on the circumference that the line
        intersects
         */
        Vector2 direction = Vector2.Normalize(axis) * radius;

        Vector2 pointA = centerC + direction;
        Vector2 pointB = centerC - direction;

        // Get perpendicular projection of both points onto axis using dot product
        min = Vector2.Dot(pointA, axis);
        max = Vector2.Dot(pointB, axis);

        if (min > max)
        {
            // Swap
            min = min + max;
            max = min - max;
            min = min - max;
        }

    }

    // Find vertex with minimum distance from circle center
    public static int FindClosestPolygonVertex(Vector2 centerC, Vector2[] vertices)
    {
        int index = -1;
        float min = float.MaxValue; // minimum distance

        // Find minimum distance
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 vertex = vertices[i];
            float distance = Vector2.Distance(vertex, centerC);

            if (distance < min)
            {
                min = distance;
                index = i;
            }
        }

        // Return index of vertex with minimum distance
        return index;
    }

    // Project vertices on normal axis based on Seperating Axis Theorem
    public static void ProjectVertices(Vector2[] vertices, Vector2 axis, out float min, out float max)
    {
        // Minimum and maximum projection
        min = float.MaxValue;
        max = float.MinValue;

        // Get perpendicular projection of every vertex onto axis using dot product
        foreach (Vector2 vertex in vertices)
        {
            float projection = Vector2.Dot(vertex, axis);

            // Find the minimum and maximum projection for a polygon
            if (projection < min) { min = projection; }
            if (projection > max) { max = projection; }
        }
    }

    // Find contact points on polygon
    public static void FindContactPoints(PhysicsBody2D bodyA, PhysicsBody2D bodyB, out Vector2 cpoint1, out Vector2 cpoint2, out int ccount)
    {
        cpoint1 = Vector2.Zero;
        cpoint2 = Vector2.Zero;
        ccount = 0;

        if (bodyA.Shape == bodyB.Shape)
        {
            // Circle Circle
            if (bodyA.Shape is ShapeType.Circle)
            {
                FindContactPoint(bodyA.Transform.Position, bodyA.Dimensions.Radius, bodyB.Transform.Position, out cpoint1);
                ccount = 1;
            }

            // Box - Box
            else
            {

            }
        }
        else
        {
            // Circle - Box
            if (bodyA.Shape is ShapeType.Circle)
            {

            }

            // Box - Circle
            else
            {
                
            }
        }
    }

    // Find contact point on circle
    public static void FindContactPoint(Vector2 centerA, float radiusA, Vector2 centerB, out Vector2 cpoint)
    {
        Vector2 direction = Vector2.Normalize(centerB - centerA);
        cpoint = centerA + direction * radiusA;
    }

    public static void UpdateCollisionState(PhysicsBody2D body, List<PhysicsBody2D> allBodies)
    {
        // Reset all collision-related properties to false initially
        body.IsOnCeiling = false;
        body.IsOnFloor = false;
        body.IsOnWallL = false;
        body.IsOnWallR = false;

        // Set collision states based on the current position and velocity of the body
        // You may need to adjust the conditions based on your specific requirements
        foreach (PhysicsBody2D otherBody in allBodies)
        {
            if (otherBody != body)
            {
                // Check if there is contact with the other body
                Vector2 normal;
                float depth;
                if (CollisionDetection.CheckCollision(body, otherBody, out normal, out depth))
                {
                    switch (normal.Y)
                    {
                        case -1:
                            body.IsOnCeiling = true;
                            break;

                        case 1:
                            body.IsOnFloor = true;
                            break;

                        default: break;
                    }

                    switch (normal.X)
                    {
                        case -1:
                            body.IsOnWallL = true;
                            break;

                        case 1:
                            body.IsOnWallR = true;
                            break;

                        default: break;
                    }
                }
            }
        }
    }
}