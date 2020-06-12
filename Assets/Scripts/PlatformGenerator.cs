using System;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    private const float DISTANCE_TO_GENERATE = Platform.BASE_LENGTH * 4;
    private const float PLAYER_OFFSET = 25.0f;
    private const float PLATFORM_OFFSET = -200.0f;

    private uint lengthCount, index;
    private Queue<GameObject> platforms;

    [SerializeField] private GameObject player;

    private PlatformGenerator()
    {
        lengthCount = index = 0;
        platforms = new Queue<GameObject>();
    }

    private void Awake()
    {
        for (int i = 0; i < 4; i++)
            platforms.Enqueue(SpawnPlatform());
    }

    private void Update()
    {
        if (CalculateZ(0) - player.transform.position.z < DISTANCE_TO_GENERATE)
            platforms.Enqueue(SpawnPlatform());

        if (CalculateOffset(player.transform.position.z) >= index + Platform.LONGEST_PLATFORM_OFFSET)
        {
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
        lengthCount += platform.offset;

        return gameObject;
    }

    private Platform GeneratePlatform()
    {
        System.Random random = new System.Random();
        double value = random.NextDouble();

        // 50% to get Flatland
        if (value < 0.5)
            return new Flatland();

        // 25% to get Highway Bridge
        else if (value < 0.75)
            return new HighwayBridge();

        // 25% to get Small Bridge
        return new SmallBridge();
    }

    private float CalculateZ(uint offset)
    {
        return Platform.BASE_LENGTH * (lengthCount + offset);
    }

    private int CalculateOffset(float location)
    {
        return (int)Math.Floor((location + PLAYER_OFFSET + PLATFORM_OFFSET) / Platform.BASE_LENGTH);
    }
}
