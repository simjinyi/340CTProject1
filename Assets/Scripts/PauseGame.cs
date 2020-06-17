using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public Button ResumeButton;
    public Button ReplayButton;
    public Button QuitButton;
    //public Button pauseButton;

    public void ResumeGame()
    {
        // idk how to write this part......
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 4);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }

    /*public void PauseGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }*/

    // Start is called before the first frame update
    void Start()
    {
        ResumeButton.onClick.AddListener(ResumeGame);
        ReplayButton.onClick.AddListener(ReplayGame);
        //pauseButton.onClick.AddListener(PauseGame);
        QuitButton.onClick.AddListener(QuitGame);
    }

}
