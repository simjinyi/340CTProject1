using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public Button ResumeButton;
    public Button ReplayButton;
    public Button QuitButton;
    public Button PauseButton;

    public void ResumeGame()
    {
        SceneManager.UnloadScene("PauseGame");
        Time.timeScale = 1f;
    }

    public void ReplayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        //Debug.Log("Quit Game!");
        //Application.Quit();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void Pause()
    {
        SceneManager.LoadScene("PauseGame", LoadSceneMode.Additive);
        Time.timeScale = 0f;
              
    }

    // Start is called before the first frame update
    void Start()
    {
        if (ResumeButton != null)
        {
            ResumeButton.onClick.AddListener(ResumeGame);
        }

        if (ReplayButton != null)
        {
            ReplayButton.onClick.AddListener(ReplayGame);
        }

        if (PauseButton != null)
        {
            PauseButton.onClick.AddListener(Pause);
        }

        if (QuitButton != null)
        {
            QuitButton.onClick.AddListener(QuitGame);
        }
    }

}
