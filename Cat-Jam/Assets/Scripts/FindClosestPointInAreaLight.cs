using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClosestPointFinder
{
    public static Vector3 FindClosestPoint(Vector3 center, Vector2 areaSize, Vector3 externalPoint)
    {
        Vector3 halfExtents = new Vector3(areaSize.x * 0.5f, areaSize.y * 0.5f, 0f);
        Vector3 difference = externalPoint - center;

        Vector3 clampedDifference = new Vector3(
            Mathf.Clamp(difference.x, -halfExtents.x, halfExtents.x),
            Mathf.Clamp(difference.y, -halfExtents.y, halfExtents.y),
            Mathf.Clamp(difference.z, -halfExtents.z, halfExtents.z)
        );

        Vector3 closestPoint = center + clampedDifference;

        return closestPoint;
    }

    // Vector3 result = FindClosestPoint();
}
