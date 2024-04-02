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
}