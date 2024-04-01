using PhysicsEngine.src.body;
using Raylib_cs;
using System.Numerics;

namespace PhysicsEngine.src.physics;

public static class Collisions
{

    /* Collisions using seperating axis theorem */
    private static bool CircPolyCollision(Vector2 centerC, float radius, Vector2 centerP, Vector2[] vertices, 
        out Vector2 normal, out float depth)
    {
        // Initializing variables for calculation
        normal = Vector2.Zero;
        depth = float.MaxValue;

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
            ProjectVertices(vertices, axis, out minA, out maxA);
            ProjectCircle(centerC, radius, axis, out minB, out maxB);

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
        Vector2 closestVertex = vertices[FindClosestPolygonVertex(centerC, vertices)];
        axis = closestVertex - centerC;
        axis = Vector2.Normalize(axis);

        // Project vertices to the normal axis
        ProjectVertices(vertices, axis, out minA, out maxA);
        ProjectCircle(centerC, radius, axis, out minB, out maxB);

        // No collision
        if (minA >= maxB || minB >= maxA)
            return false;

        axisDepth = MathF.Min(maxB - minA, maxA - minB);

        if (axisDepth < depth)
        {
            depth = axisDepth;
            normal = axis;
        }

        // Direction from polygon to circle center
        Vector2 direction = centerP - centerC;

        // Correct normal based on direction
        normal = Vector2.Dot(direction, normal) < 0f ? -normal : normal;

        // Collision detected
        return true;
    }

    // Find vertex with minimum distance from circle center
    private static int FindClosestPolygonVertex(Vector2 centerC, Vector2[] vertices)
    {
        int index = -1;
        float min = float.MaxValue; // minimum distance

        // Find minimum distance
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 vertex = vertices[i];
            float distance = Vector2.Distance(vertex, centerC);

            if (distance < min) {
                min = distance;
                index = i;
            }
        }

        // Return index of vertex with minimum distance
        return index;
    }

    private static void ProjectCircle(Vector2 centerC, float radius, Vector2 axis, out float min, out float max)
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

        if (min > max) {
            // Swap
            min = min + max;
            max = min - max;
            min = min - max;
        }

    }

    private static bool PolygonCollisions(Vector2 centerA, Vector2[] verticesA, Vector2 centerB, Vector2[] verticesB, 
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
            axis = Vector2.Normalize(axis);

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

        // Collision
        return true;
    }

    // Project vertices on normal axis based on Seperating Axis Theorem
    private static void ProjectVertices(Vector2[] vertices, Vector2 axis, out float min, out float max)
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
    public static void HandleCollision(List<PhysicsBody2D> bodies)
    {
        float accumulator = 0f;
        float timestep = Raylib.GetFrameTime();

        while (accumulator < timestep)
        {
            for (int i = 0; i < bodies.Count; i++)
            {
                PhysicsBody2D bodyA = bodies[i];
                for (int j = i + 1; j < bodies.Count; j++)
                {
                    PhysicsBody2D bodyB = bodies[j];

                    // Resolve collision 
                    if (CheckCollision(bodyA, bodyB, out Vector2 normal, out float depth))
                    {
                        ResolveCollision(bodyA, bodyB, normal, depth);
                    }
                    else continue;
                }
            }

            accumulator += timestep;
        }
    }

    private static bool CheckCollision(PhysicsBody2D bodyA, PhysicsBody2D bodyB, out Vector2 normal, out float depth)
    {
        bool collision = false;

        // Check collision between bodies of different shapes
        if (bodyA.Shape != bodyB.Shape)
        {
            if (bodyA.Shape == ShapeType.Circle)
            {
                collision = CircPolyCollision(bodyA.Transform.Position, bodyA.Dimensions.Radius,
                    bodyB.Transform.Position, bodyB.GetTransformedVertices(), out normal, out depth);
            }

            else
            {
                collision = CircPolyCollision(bodyB.Transform.Position, bodyB.Dimensions.Radius,
                    bodyA.Transform.Position, bodyA.GetTransformedVertices(), out normal, out depth);
            }
        }

        // Check collision between bodies of the same shapes
        else
        {
            // Circle - Circle Collision
            if (bodyA.Shape == ShapeType.Circle)
            {
                collision = CicrcleCollisions(bodyA.Transform.Position, bodyA.Dimensions.Radius,
                                      bodyB.Transform.Position, bodyB.Dimensions.Radius,
                                      out normal, out depth);
            }

            // Polygon - Polygon Collision
            else
            {
                collision = PolygonCollisions(bodyA.Transform.Position, bodyA.GetTransformedVertices(),
                    bodyB.Transform.Position, bodyB.GetTransformedVertices(), out normal, out depth);
            }

        }

        return collision;
    }

    private static void ResolveCollision(PhysicsBody2D bodyA, PhysicsBody2D bodyB, Vector2 normal, float depth)
    {
        // Relative velocity of the 2 bodies
        Vector2 velocity = bodyB.LinVelocity - bodyA.LinVelocity;

        // Restitution of bodies
        float restitution = MathF.Min(bodyA.Substance.Restitution, bodyB.Substance.Restitution);

        // Impulse of collisions
        float impulse = -((1 + restitution) * Vector2.Dot(velocity, normal)) / ((1f / bodyA.Substance.Mass) + (1f / bodyB.Substance.Mass));

        // Calculate velocity after collision
        bodyA.LinVelocity -= impulse / bodyA.Substance.Mass * normal;
        bodyB.LinVelocity += impulse / bodyB.Substance.Mass * normal;

        // Calculate the direction each body needs to be pushed in
        Vector2 direction = normal * depth * 0.5f;

        // Adjust direction for circle and circle collisions
        direction *=
            (bodyA.Shape is ShapeType.Circle && bodyB.Shape is ShapeType.Circle ||
            bodyA.Shape is ShapeType.Box && bodyB.Shape is ShapeType.Circle) ? -1f : 1f;

        bodyA.Translate(-direction);
        bodyB.Translate(direction);
    }

}

