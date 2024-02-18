using PhysicsEngine.src.body;
using System.Numerics;

namespace PhysicsEngine.src.physics;

public static class Collisions
{

    /* Polygon collisions using seperating axis theorem */
    private static bool PolygonCollisions(Vector2[] verticesA, Vector2[] verticesB, 
        out Vector2 normal, out float depth)
    {
        // Collision normal and depth
        normal = Vector2.Zero;
        depth = float.MaxValue;

        // Polygon 1 
        for (int i = 0; i < verticesA.Length; i++)
        {
            // Get a pair of vertices from polygon 1
            Vector2 vertexA = verticesA[i];
            Vector2 vertexB = verticesA[(i + 1) % verticesA.Length];

            // Get an edge and it's perpendicular axis
            Vector2 edge = vertexB - vertexA;
            Vector2 axis = new Vector2(-edge.Y, edge.X);

            // Project vertices to the normal axis
            ProjectVertices(verticesA, axis, out float minA, out float maxA);
            ProjectVertices(verticesB, axis, out float minB, out float maxB);

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
            ProjectVertices(verticesA, axis, out float minA, out float maxA);
            ProjectVertices(verticesB, axis, out float minB, out float maxB);

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

        // Get the collision normal and depth
        depth /= normal.Length();
        normal = Vector2.Normalize(normal);

        // Center of polygons
        Vector2 centerA = GetPolygonCenter(verticesA);
        Vector2 centerB = GetPolygonCenter(verticesB);

        // Direction from centerB to centerA
        Vector2 direction = centerB - centerA;

        // Correct normal based on direction
        normal = Vector2.Dot(direction, normal) < 0f ? -normal : normal;

        // Collision
        return true;
    }

    // Get the center of polygon shape using arithmetic mean
    private static Vector2 GetPolygonCenter(Vector2[] vertices) 
    {
        float sumX = 0f;
        float sumY = 0f;

        float centerX;
        float centerY;

        // Get sum of x and y coordinates for vertices
        for (int i = 0; i < vertices.Length; i++) {

            Vector2 vertex = vertices[i];
            sumX += vertex.X;
            sumY += vertex.Y;
        }

        // Calculate centers
        centerX = sumX / vertices.Length;
        centerY = sumY / vertices.Length; 
        
        // Return the coordinates of center as a 2D vector
        return new Vector2(centerX, centerY);
    }

    // Project vertices on normal axis based on Seperating Axis Theorem
    private static void ProjectVertices(Vector2[] vertices, Vector2 axis, out float min, out float max)
    {
        // Minimum and maximum projection
        min = float.MaxValue;
        max = float.MinValue;

        // Project every vertex onto normal axis using dot product
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 vertex = vertices[i];
            float projection = Vector2.Dot(vertex, axis);

            // Find the minimum and maximum projection for a polygon
            if (projection < min) { min = projection; }
            if (projection > max) { max = projection; }
        }
    }


    private static bool CicrcleCollisions(Vector2 centerA, float radiusA, 
        Vector2 centerB, float radiusB,
        out Vector2 normal, out float depth
        )
    {
        // Get distance between the circles and sum of radii
        float distance = Vector2.Distance(centerA, centerB);
        float totalRadii = radiusA + radiusB;

        // Get normal and depth of collision
        normal = Vector2.Normalize(centerA - centerB);
        depth = totalRadii - distance;

        // Return true if collision occured
        return distance < totalRadii ? true : false;
    }

    // Move objects when they collide
    public static void HandleCollision(List<RigidBody2D> bodies)
    {
        for (int i = 0; i < bodies.Count; i++)
        {
            RigidBody2D bodyA = bodies[i];

            for (int j = i + 1; j < bodies.Count; j++)
            {
                RigidBody2D bodyB = bodies[j];

                // Collision normal and depth
                Vector2 normal;
                float depth;

                // Check for polygon collisions
                if (PolygonCollisions(
                    bodyA.GetTransformedVertices(), 
                    bodyB.GetTransformedVertices(),
                    out normal, out depth)
                    )
                {
                    // Calculate the direction each body needs to be pushed in
                    Vector2 pushDirection = normal * depth * 0.5f;

                    // Move objects
                    bodyA.Move(-pushDirection);
                    bodyB.Move(pushDirection);
                }

                // Check for circle collision
                if (CicrcleCollisions(
                    bodyA.Position, bodyA.Radius,
                    bodyB.Position, bodyB.Radius,
                    out normal, out depth))
                {

                    // Calculate the direction each body needs to be pushed in
                    Vector2 pushDirection = normal * depth * 0.5f;

                    // Move objects
                    bodyA.Move(pushDirection);
                    bodyB.Move(-pushDirection);
                }
            }
        }
    }

}

