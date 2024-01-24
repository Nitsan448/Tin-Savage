using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathExtensions
{
    public static int Sign(this float input)
    {
        return input switch
        {
            < 0 => -1,
            > 0 => 1,
            _ => 0
        };
    }

    public static int Sign(this int input)
    {
        return input switch
        {
            < 0 => -1,
            > 0 => 1,
            _ => 0
        };
    }
}