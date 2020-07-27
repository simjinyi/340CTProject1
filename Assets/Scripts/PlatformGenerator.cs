using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    private const float DISTANCE_TO_GENERATE = Platform.BASE_LENGTH * 4;
    private const float PLAYER_OFFSET = 25.0f;
    private const float PLATFORM_OFFSET = -200.0f;

    private uint lengthCount, index;
    private Queue<GameObject> platforms;

    public GameObject player;

    private PlatformGenerator()
    {
        lengthCount = index = 0;
        platforms = new Queue<GameObject>();
    }

private void Awake()
{
    // Add four platforms in the beginning
    for (int i = 0; i < 4; i++)
        platforms.Enqueue(SpawnPlatform());
}

private void Update()
{
    // If the player is now close to the end, generate a new platform
    if (CalculateZ(0) - player.transform.position.z < DISTANCE_TO_GENERATE)
        platforms.Enqueue(SpawnPlatform());

    // Calculate the distance of the player from the previous platform
    if (CalculateOffset(player.transform.position.z) >= index + Platform.LONGEST_PLATFORM_OFFSET)
    {
        // If the distance is long enough from the previous platform, remove it to reduce memory usage
        Destroy(platforms.Dequeue(), 1.0f);
        Debug.Log("Destroyed");
        index += Platform.LONGEST_PLATFORM_OFFSET;
    }
}

    private GameObject SpawnPlatform()
    {
        Platform platform = GeneratePlatform();

        // Instantiate the platform at the vector3 location without rotation (Quarternion.identity)
        GameObject gameObject = Instantiate(platform.gameObject, new Vector3(platform.vector2.x, platform.vector2.y, CalculateZ(platform.offset)), Quaternion.identity);

        // The lengthCount will be used to calculate the coordinate to populate
        lengthCount += platform.offset;

        return gameObject;
    }

    private Platform GeneratePlatform()
    {
        // Instantiate a platform randomly
        System.Random random = new System.Random();
        double value = random.NextDouble();

        // 60% to get Flatland
        if (value < 0.6)
            return new Flatland();

        // 20% to get Highway Bridge
        else if (value < 0.80)
            return new HighwayBridge();

        // 20% to get Small Bridge
        return new SmallBridge();
    }

    private float CalculateZ(uint offset)
    {
        // Convert the Z position to populate the platform
        return Platform.BASE_LENGTH * (lengthCount + offset);
    }

    private int CalculateOffset(float location)
    {
        // Get the offset (the lengthCount) from the location of the player
        return (int)Math.Floor((location + PLAYER_OFFSET + PLATFORM_OFFSET) / Platform.BASE_LENGTH);
    }
}
