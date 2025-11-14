using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static void FlipLeft(this Transform t)
    {
        Vector3 scale = t.localScale;
        scale.x = Mathf.Abs(scale.x) * -1; 
        t.localScale = scale;
    }

    public static void FlipRight(this Transform t)
    {
        Vector3 scale = t.localScale;
        scale.x = Mathf.Abs(scale.x); 
        t.localScale = scale;
    }

    public static void FlipTowards(this Transform t, float horizontal)
    {
        if (horizontal < 0) t.FlipLeft();
        else if (horizontal > 0) t.FlipRight();
    }

    public static void LookAtTarget(this Transform t, Transform target)
    {
        if (target.position.x > t.position.x)
            t.FlipRight();
        else if (target.position.x < t.position.x)
            t.FlipLeft();
    }
}
