using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    public Button CreditButton;
    public Button InstructionButton;
    public Button SettingsButton;

    public void CreditToMain()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void InstructionToMain()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void SettingsToMain()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        if (CreditButton != null)
        {
            CreditButton.onClick.AddListener(CreditToMain);
        }

        if (InstructionButton != null)
        {
            InstructionButton.onClick.AddListener(InstructionToMain);
        }

        if (SettingsButton != null)
        {
            SettingsButton.onClick.AddListener(SettingsToMain);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CreditButton != null)
                CreditButton.onClick.Invoke();

            if (InstructionButton != null)
                InstructionButton.onClick.Invoke();

            if (SettingsButton != null)
                SettingsButton.onClick.Invoke();
        }
    }
}
