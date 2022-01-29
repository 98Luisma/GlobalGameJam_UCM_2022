using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// Extension methods for various Unity classes.
///</summary>
public static class ExtensionMethods
{
    ///<summary>
    /// Returns whether the position is visible by the camera.
    /// Uses a (0.1, 0.1, 0.1) check size by default.
    ///</summary>
    public static bool IsPointInBounds(this Camera camera, Vector3 worldPos)
    {
        return camera.IsPointInBounds(worldPos, new Vector3(0.1f, 0.1f, 0.1f));
    }

    ///<summary>
    /// Returns whether the position is visible by the camera.
    ///</summary>
    public static bool IsPointInBounds(this Camera camera, Vector3 worldPos, Vector3 checkSize)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        Bounds bounds = new Bounds(worldPos, checkSize);
        return GeometryUtility.TestPlanesAABB(planes, bounds);
    }

    ///<summary>
    /// Returns the position of a corner of the camera frustum at
    /// the given zDepth from the camera.
    /// Corners are clock-wise starting from the bottom left.
    ///</summary>
    public static Vector3 GetFrustumCorner(this Camera camera, int corner, float zDepth)
    {
        Vector3[] corners = new Vector3[4];
        camera.CalculateFrustumCorners(new Rect(0f, 0f, 1f, 1f), zDepth, Camera.MonoOrStereoscopicEye.Mono, corners);
        
        return camera.transform.TransformVector(corners[corner]);
    }

    ///<summary>
    /// Returns a random point inside the camera frustum at the given
    /// zDepth from the camera position.
    ///</summary>
    public static Vector3 RandomPointInFrustum(this Camera camera, float zDepth)
    {
        Vector3[] corners = new Vector3[4];
        camera.CalculateFrustumCorners(new Rect(0f, 0f, 1f, 1f), zDepth, Camera.MonoOrStereoscopicEye.Mono, corners);
        Vector3 bottomLeftCorner = camera.transform.TransformVector(corners[0]);
        Vector3 topRightCorner = camera.transform.TransformVector(corners[2]);

        Vector3 randomPoint = new Vector3(
            Random.Range(bottomLeftCorner.x, topRightCorner.x),
            Random.Range(bottomLeftCorner.y, topRightCorner.y),
            Random.Range(bottomLeftCorner.z, topRightCorner.z)
        );

        /*
        Debug.DrawLine(camera.transform.position, bottomLeftCorner, Color.red, 2f);
        Debug.DrawLine(camera.transform.position, topRightCorner, Color.green, 2f);
        Debug.DrawLine(camera.transform.position, randomPoint, Color.blue, 2f);
        */
        
        return randomPoint;
    }
}