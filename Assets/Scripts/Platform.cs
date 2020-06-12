using UnityEngine;

public abstract class Platform
{
    public const float BASE_LENGTH = 400.0f;
    public const uint LONGEST_PLATFORM_OFFSET = 2;

    public Vector2 vector2 { get; }
    public uint offset { get; }
    public GameObject gameObject { get; }

    public Platform(Vector2 vector2, uint offset, GameObject gameObject)
    {
        this.vector2 = vector2;
        this.offset = offset;
        this.gameObject = gameObject;
    }
}
