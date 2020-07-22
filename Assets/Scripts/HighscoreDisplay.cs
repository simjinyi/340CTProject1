using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreDisplay : MonoBehaviour
{
    public Text highscoreText;

    void Start()
    {
        highscoreText.text = "Highscore: " + DataPersistence.GetHighScore();
    }
}
