using UnityEngine;

public class MathematicsGenerator : MonoBehaviour
{
    private const string SPAWNPOINT_TAG = "Spawnpoint";

    // Update is called once per frame
    void Update()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(SPAWNPOINT_TAG);
        Debug.Log(gameObjects.Length);
    }
}
