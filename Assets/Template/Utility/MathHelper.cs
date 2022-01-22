using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class MathHelper
{
    public static Vector2 GetRemainder(Vector2 input)
    {
        Vector2 remainder = Vector2.zero;
        if (input.x > 0) remainder.x = input.x - Mathf.FloorToInt(input.x);
        else remainder.x = input.x - Mathf.CeilToInt(input.x);

        if (input.y > 0) remainder.y = input.y - Mathf.FloorToInt(input.y);
        else remainder.y = input.y - Mathf.CeilToInt(input.y);

        return remainder;
    }

    public static double Clamp(double val, double min, double max)
    {
        return Math.Max(Math.Min(val, max), min);
    }

    public static bool IsNeg(int val)
    {
        if (val < 0)
            return true;
        return false;
    }

    public static bool IsNeg(double val)
    {
        if (val < 0)
            return true;
        return false;
    }

    public static int ReverseBytes(int val)
    {
        var b1 = (val >> 0) & 0xff;
        var b2 = (val >> 8) & 0xff;
        var b3 = (val >> 16) & 0xff;
        var b4 = (val >> 24) & 0xff;

        return b1 << 24 | b2 << 16 | b3 << 8 | b4 << 0;
    }
}
