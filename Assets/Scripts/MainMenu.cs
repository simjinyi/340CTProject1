using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button btnStart;
    public Button btnInstruction;
    public Button btnCredit;
    public Button btnSettings;
    public Button btnQuit;

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadInstruction()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }

    public void LoadCredit()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }


    public void LoadSettings()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);
    }

    public void Start()
    {
        btnStart.onClick.AddListener(StartGame);
        btnInstruction.onClick.AddListener(LoadInstruction);
        btnCredit.onClick.AddListener(LoadCredit);
        btnSettings.onClick.AddListener(LoadSettings);
        btnQuit.onClick.AddListener(ExitGame);
    }

    public void ExitGame()
    {
        Debug.Log("Quit Game!");
        //Application.Quit();
    }
}
