using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Text currentScoreText;
    public Text highScoreText;

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
        currentScoreText.text = "Current Score: " + DataPersistence.GetPreviousScore().ToString();
        highScoreText.text = "Highscore: " + DataPersistence.GetHighScore().ToString();

        ReplayGameOverButton.onClick.AddListener(ReplayGameOver);
        QuitGameOverButton.onClick.AddListener(QuitGameOver);
    }
}
