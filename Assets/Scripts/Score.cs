using UnityEngine;
using UnityEngine.UI;

public class Score
{
    private readonly Transform player;
    private readonly Text scoreText;
    private readonly Text multiplierText;
    private readonly int initialMultiplier;
    private float score;
    private int multiplier;

    private float prevPosition;

    // Update is called once per frame
    public Score(Transform player, Text scoreText, Text multiplierText, int multiplier)
    {
        this.player = player;
        this.scoreText = scoreText;
        this.multiplier = initialMultiplier = multiplier;
        this.multiplierText = multiplierText;

        score = 0;
        this.scoreText.text = player.position.z.ToString("0");
        this.multiplierText.text = multiplier + "x Multiplier";

        prevPosition = 0;
    }

    public void UpdateScore()
    {
        float z = player.position.z;

        if (z < 0)
        {
            scoreText.text = "0";
            return;
        }

        float increment = z - prevPosition;
        prevPosition = z;

        score += increment * multiplier / 10;
        scoreText.text = ((int) score).ToString();
    }

    public void IncrementMultiplier()
    {
        multiplierText.text = ++multiplier + "x Multiplier";
    }

    public void ResetMultiplier()
    {
        multiplierText.text = (multiplier = initialMultiplier) + "x Multiplier";
    }

    public int GetScore()
    {
        return (int) score;
    }
}
