using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    private const float DISTANCE_TO_GENERATE = Platform.BASE_LENGTH * 4;

    private uint lengthCount;
    [SerializeField] private GameObject player;

    private PlatformGenerator()
    {
        lengthCount = 0;
    }

    private void Awake()
    {
        for (int i = 0; i < 4; i++)
            SpawnPlatform();
    }

    private void Update()
    {
        if (CalculateZ(0) - player.transform.position.z < DISTANCE_TO_GENERATE)
            SpawnPlatform();
    }

    private void SpawnPlatform()
    {
        Platform platform = GeneratePlatform();

        // Instantiate the platform at the vector3 location without rotation (Quarternion.identity)
        Instantiate(platform.gameObject, new Vector3(platform.vector2.x, platform.vector2.y, CalculateZ(platform.offset)), Quaternion.identity);

        lengthCount += platform.offset;
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
}
