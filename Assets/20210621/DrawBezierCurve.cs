using System.Collections.Generic;
using UnityEngine;

public static class DrawBezierCurve
{
    public static List<Vector3> BezierCurveWithThree(Vector3 p0, Vector3 p1, Vector3 p2, int vertexCount)
    {
        List<Vector3> pointList = new List<Vector3>();
        pointList.Clear();
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            Vector3 p01 = Vector3.Lerp(p0, p1, ratio);
            Vector3 p12 = Vector3.Lerp(p1, p2, ratio);
            Vector3 p012 = Vector3.Lerp(p01, p12, ratio);
            pointList.Add(p012);
        }
        pointList.Add(p2);
        return pointList;
    }

    public static List<Vector3> BezierCurveWithUnlimitPoints(Vector3[] positions, int vertexCount)
    {
        List<Vector3> pointList = new List<Vector3>();
        pointList.Clear();

        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount)
        {
            pointList.Add(UnlimitBezierCurve(positions, ratio));
        }
        pointList.Add(positions[positions.Length - 1]);
        return pointList;
    }

    private static Vector3 UnlimitBezierCurve(Vector3[] positions, float ratio)
    {
        Vector3[] temp = new Vector3[positions.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i] = positions[i];
        }
        int n = temp.Length - 1;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n - i; j++)
            {
                temp[j] = Vector3.Lerp(temp[j], temp[j + 1], ratio);
            }
        }
        return temp[0];
    }
}
