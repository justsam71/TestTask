
using UnityEngine;

public class PlayerShootEvent
{
    public Vector2 Position { get; }
    public Vector2 Direction { get; }
    public Transform Target { get; }

    public PlayerShootEvent(Vector3 position, Vector3 direction, Transform target)
    {
        Position = position;
        Direction = direction;
        Target = target;
    }
}