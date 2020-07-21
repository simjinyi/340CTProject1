using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        for (int i = 0; i < 4; i++)
            platforms.Enqueue(SpawnPlatform());

        InvokeRepeating("IncrementSpeed", 0, 5.0f);
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

    private void IncrementSpeed()
    {
        player.GetComponent<PlayerMovement>().IncrementSpeed();
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
        return Platform.BASE_LENGTH * (lengthCount + offset);
    }

    private int CalculateOffset(float location)
    {
        return (int)Math.Floor((location + PLAYER_OFFSET + PLATFORM_OFFSET) / Platform.BASE_LENGTH);
    }
}
