using UnityEngine;
using UnityEngine.UI;

public class Score
{
    private readonly Transform player;
    private readonly Text scoreText;
    private readonly Text multiplierText;
    private readonly int multiplier;
    private int score;

    // Update is called once per frame
    public Score(Transform player, Text scoreText, Text multiplierText, int multiplier)
    {
        this.player = player;
        this.scoreText = scoreText;
        this.multiplier = multiplier;
        this.multiplierText = multiplierText;

        score = 0;
        this.scoreText.text = player.position.z.ToString("0");
        this.multiplierText.text = multiplier + "x Multiplier";
    }

    public void UpdateScore()
    {
        float z = player.position.z;

        if (z < 0)
        {
            scoreText.text = "0";
            return;
        }

        score = (int) z * multiplier;
        scoreText.text = score.ToString();
    }

    public int GetScore()
    {
        return score;
    }
}
