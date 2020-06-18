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
        // idk how to write this part......
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }

    public void Pause()
    {    
        SceneManager.LoadScene("PauseGame");      
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
