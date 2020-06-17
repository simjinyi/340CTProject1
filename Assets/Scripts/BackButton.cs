using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    public Button backCredit;
    public Button backInstruction;
    public Button backSettings;

    public void CreditToMain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

    public void InstructionToMain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
    }

    public void SettingsToMain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 4);
    }

    // Start is called before the first frame update
    void Start()
    {
        backCredit.onClick.AddListener(CreditToMain);
        backInstruction.onClick.AddListener(InstructionToMain);
        backSettings.onClick.AddListener(SettingsToMain);
    }

}
