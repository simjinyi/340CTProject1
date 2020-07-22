using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    private static bool isGamePaused = false;

    public GameObject pauseMenu;
    public Button mainMenuButton;
    public Button resumeButton;
    public Button restartButton;
    public Button muteButton;
    public Button pauseButton;

    private bool isMuted;

    public Gameplay gameplay;

    private void Start()
    {
        isMuted = DataPersistence.GetMute();
        Play();

        muteButton.GetComponentInChildren<Text>().text = isMuted ? "Sound Off" : "Sound On";

        pauseButton.onClick.AddListener(Pause);
        resumeButton.onClick.AddListener(Play);

        mainMenuButton.onClick.AddListener(() =>
        {
            gameplay.UpdateHighscore();
            SceneManager.LoadScene("MainMenuScene");
        });

        restartButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene");
        });

        muteButton.onClick.AddListener(() =>
        {
            if (isMuted)
            {
                DataPersistence.SetMute(false);
                muteButton.GetComponentInChildren<Text>().text = "Sound On";
                isMuted = false;
            }
            else
            {
                DataPersistence.SetMute(true);
                muteButton.GetComponentInChildren<Text>().text = "Sound Off";
                isMuted = true;
            }
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
                Play();
            else
                Pause();
        }
    }

    private void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        isGamePaused = true;
    }

    private void Play()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        isGamePaused = false;
    }
}
