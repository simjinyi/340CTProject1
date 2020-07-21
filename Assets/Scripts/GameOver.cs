using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Button ReplayGameOverButton;
    public Button QuitGameOverButton;

    public void ReplayGameOver()
    {
        Debug.Log("Replay");
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGameOver()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    void Start()
    {

        ReplayGameOverButton.onClick.AddListener(ReplayGameOver);
        QuitGameOverButton.onClick.AddListener(QuitGameOver);



    }
}
