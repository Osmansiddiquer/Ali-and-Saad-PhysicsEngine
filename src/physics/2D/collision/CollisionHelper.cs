using PhysicsEngine.src.physics._2D.body;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;

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

    private static void PointSegmentDistance(Vector2 p, Vector2 a, Vector2 b, out float distanceSquared, out Vector2 contact)
    {
        Vector2 ab = b - a;
        Vector2 ap = p - a;

        float projection = Vector2.Dot(ab, ap);
        float d = projection / Vector2.DistanceSquared(ab, Vector2.Zero);

        if (d <= 0f)
            contact = a;
        else if (d >= 1f)
            contact = b;
        else 
            contact = a + ab * d;

        distanceSquared = Vector2.DistanceSquared(p, contact);
    }

    // Find contact points on polygon / circle
    public static void FindContactPoints(PhysicsBody2D bodyA, PhysicsBody2D bodyB, out Vector2 cpoint1, out Vector2 cpoint2, out int ccount)
    {
        cpoint1 = Vector2.Zero;
        cpoint2 = Vector2.Zero;
        ccount = 0;

        Vector2 centerA = bodyA.Transform.Position;
        Vector2 centerB = bodyB.Transform.Position;

        if (bodyA.Shape == bodyB.Shape)
        {
            // Circle - Circle
            if (bodyA.Shape == ShapeType.Circle)
            {
                float radius = bodyA.Dimensions.Radius;

                Vector2 direction = Vector2.Normalize(centerB - centerA);
                cpoint1 = centerA + direction * radius;

                ccount = 1;
            }

            // Box - Box
            else
            {
                Vector2[] verticesA = bodyA.GetTransformedVertices();
                Vector2[] verticesB = bodyB.GetTransformedVertices();

                float minDistanceSquared = float.MaxValue;

                for (int i = 0; i < verticesA.Length; i++)
                {
                    Vector2 point = verticesA[i];

                    for (int j = 0; j < verticesB.Length; j++)
                    {
                        Vector2 vertexA = verticesB[j];
                        Vector2 vertexB = verticesB[(j + 1) % verticesB.Length];

                        PointSegmentDistance(point, vertexA, vertexB, out float distanceSquared, out Vector2 cp);

                        if (distanceSquared < minDistanceSquared)
                        {
                            minDistanceSquared = distanceSquared;
                            cpoint1 = cp;
                            ccount = 1;
                        }

                        else if (MathF.Abs(distanceSquared - minDistanceSquared) < 0.0005f &&
                            !(MathF.Abs(cp.X - cpoint1.X) < 0.0005f && MathF.Abs(cp.Y - cpoint1.Y) < 0.0005f))
                        {
                            cpoint2 = cp;
                            ccount = 2;
                        }
                    }
                }

                for (int i = 0; i < verticesB.Length; i++)
                {
                    Vector2 point = verticesB[i];

                    for (int j = 0; j < verticesA.Length; j++)
                    {
                        Vector2 vertexA = verticesA[j];
                        Vector2 vertexB = verticesA[(j + 1) % verticesA.Length];

                        PointSegmentDistance(point, vertexA, vertexB, out float distanceSquared, out Vector2 cp);

                        if (distanceSquared < minDistanceSquared)
                        {
                            minDistanceSquared = distanceSquared;
                            cpoint1 = cp;
                            ccount = 1;
                        }

                        else if (MathF.Abs(distanceSquared - minDistanceSquared) < 0.0005f &&
                            !(MathF.Abs(cp.X - cpoint1.X) < 0.0005f && MathF.Abs(cp.Y - cpoint1.Y) < 0.0005f))
                        {
                            cpoint2 = cp;
                            ccount = 2;
                        }
                    }
                }
            }

        }

        // Box - Circle / Circle - Box
        else
        {
            Vector2 centerC;
            Vector2[] vertices;

            if (bodyA.Shape == ShapeType.Circle)
            {
                centerC = centerA;
                vertices = bodyB.GetTransformedVertices();
            }

            else
            {
                centerC = centerB;
                vertices = bodyA.GetTransformedVertices();
            }

            float minDistanceSquared = float.MaxValue;

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 vertexA = vertices[i];
                Vector2 vertexB = vertices[(i + 1) % vertices.Length];

                // Passes out a candidate for the contact point
                PointSegmentDistance(centerC, vertexA, vertexB, out float distanceSquared, out Vector2 cp);

                // Choose cp with min distance as our contact point
                if (distanceSquared < minDistanceSquared)
                {
                    minDistanceSquared = distanceSquared;
                    cpoint1 = cp;
                    ccount = 1;
                }
            }
        }

    }


    public static void UpdateCollisionState(PhysicsBody2D body, List<PhysicsBody2D> bodies)
    {
        // Reset all collision-related properties to false initially
        body.IsOnCeiling = false;
        body.IsOnFloor = false;
        body.IsOnWallL = false;
        body.IsOnWallR = false;

        body.Normal = Vector2.Zero;

        // Set collision states based on the current position and velocity of the body
        // You may need to adjust the conditions based on your specific requirements
        foreach (PhysicsBody2D otherBody in bodies)
        {
            if (otherBody != body)
            {
                // Check if there is contact with the other body
                Vector2 normal;
                float depth;

                if (CollisionDetection.CheckCollision(body, otherBody, out normal, out depth))
                {
                    body.IsOnCeiling = normal.Y < -0.5f;
                    body.IsOnFloor = normal.Y > 0.5f;

                    body.IsOnWallL = normal.X < -0.5f;
                    body.IsOnWallR = normal.X > 0.5f;

                    body.Normal = normal;
                }
            }
        }
    }

    public static bool AreParallel(Vector2 v1, Vector2 v2)
    {
        // Avoid division by zero
        if (v2.X == 0 || v2.Y == 0)
        {
            return v1.X == v2.X && v1.Y == v2.Y;
        }

        // Check if the ratios are equal
        return v1.X / v2.X == v1.Y / v2.Y;
    }
}
