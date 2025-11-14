using System;
using UnityEngine;

public class ShootButtonPressedEvent
{
    public Transform Target { get; private set; }

    public ShootButtonPressedEvent(Transform target)
    {
        Target = target;
    }
}