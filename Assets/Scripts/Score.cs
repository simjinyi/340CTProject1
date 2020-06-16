using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Transform player;
    public Text scoreText;

    // Update is called once per frame
    void Update()
    {
        float z = player.position.z;

        if (z < 0)
        {
            scoreText.text = "0";
            return;
        }
            
        scoreText.text = player.position.z.ToString("0");
    }
}
