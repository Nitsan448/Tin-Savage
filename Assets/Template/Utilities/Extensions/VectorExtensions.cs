using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 GetWithNewXValue(this Vector3 vector, float xValue)
    {
        return new Vector3(xValue, vector.y, vector.z);
    }

    public static Vector3 GetWithNewYValue(this Vector3 vector, float yValue)
    {
        return new Vector3(vector.x, yValue, vector.z);
    }

    public static Vector3 GetWithNewZValue(this Vector3 vector, float zValue)
    {
        return new Vector3(vector.x, vector.y, zValue);
    }
}