using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformGenerator : MonoBehaviour
{
    private const float DISTANCE_TO_GENERATE = Platform.BASE_LENGTH * 4;
    private const float PLAYER_OFFSET = 25.0f;
    private const float PLATFORM_OFFSET = -200.0f;
    private const string SPAWNPOINT_TAG = "Spawnpoint";
    private const string ANSWER_TAG = "Answer";

    private uint lengthCount, index;
    private Queue<GameObject> platforms;
    private GameObject[] spawnpoints;

    private GameObject nextSpawnpoint;
    private GameObject prevSpawnpoint;

    private QuestionGenerator questionGenerator;

    public GameObject player;
    public Text text;

    private PlatformGenerator()
    {
        lengthCount = index = 0;
        platforms = new Queue<GameObject>();
        spawnpoints = null;
        questionGenerator = new QuestionGenerator(AgeGroup._6TO8, Difficulty.EASY);
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

        //spawnpoints = GameObject.FindGameObjectsWithTag(SPAWNPOINT_TAG);

        if ((nextSpawnpoint = FindNextSpawnpoint()) != prevSpawnpoint)
        {
            prevSpawnpoint = nextSpawnpoint;
            GameObject ago = Instantiate((GameObject)Resources.Load("Prefabs/Answer"), nextSpawnpoint.transform.position, Quaternion.identity);
            GameObject[] answers = FindGameObjectsWithTag(ago, ANSWER_TAG);
            
            Question q = questionGenerator.Generate();

            text.text = q.question;
            
            for (int i = 0; i < answers.Length; i++)
                answers[i].transform.Find("text").GetComponent<TextMesh>().text = q.answers[i].Item1.ToString();
        }
    }

    private GameObject SpawnPlatform()
    {
        Platform platform = GeneratePlatform();

        // Instantiate the platform at the vector3 location without rotation (Quarternion.identity)
        GameObject gameObject = Instantiate(platform.gameObject, new Vector3(platform.vector2.x, platform.vector2.y, CalculateZ(platform.offset)), Quaternion.identity);
        lengthCount += platform.offset;

        //GameObject GAME_OBJECT = (GameObject)Resources.Load("Prefabs/Obstacle");
        //GameObject[] spawnpoints = FindGameObjectsWithTag(gameObject, "Spawnpoint");

        //if (spawnpoints.Length > 0)
        //{
        //    GameObject pb = Instantiate(GAME_OBJECT);
        //    pb.transform.SetParent(gameObject.transform);
        //    pb.transform.localPosition = nearestSpawnpoint.transform.localPosition;
        //}
        //else
        //{
        //    Debug.Log("No Spawnpoint Found");
        //}

        // spawnpoints = GameObject.FindGameObjectsWithTag(SPAWNPOINT_TAG);

        return gameObject;
    }

    private Platform GeneratePlatform()
    {
        System.Random random = new System.Random();
        double value = random.NextDouble();

        // 50% to get Flatland
        if (value < 0.5)
        {
            Debug.Log("Flatland");
            return new Flatland();
        }

        // 25% to get Highway Bridge
        else if (value < 0.75)
        {
            Debug.Log("Highway Bridge");
            return new HighwayBridge();
        }

        // 25% to get Small Bridge
        Debug.Log("Small Bridge");
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

    private GameObject[] FindGameObjectsWithTag(GameObject parent, string tag)
    {
        List<GameObject> gameObjects = new List<GameObject>();

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);

            if (child.tag == tag)
                gameObjects.Add(child.gameObject);
        }

        return gameObjects.ToArray();
    }

    private GameObject FindNextSpawnpoint()
    {
        GameObject bestTarget = null;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = player.transform.position;
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(SPAWNPOINT_TAG);

        foreach (GameObject potentialTarget in gameObjects)
        {
            if (potentialTarget.transform.position.z < currentPosition.z)
                continue;

            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    //private GameObject FindNextSpawnpoint()
    //{
    //    foreach (GameObject spawnpoint in spawnpoints)
    //        if (spawnpoint.transform.position.z >= player.transform.position.z)
    //            return spawnpoint;

    //    return null;
    //}
}
