using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Gameplay : MonoBehaviour
{
    private const string SPAWNPOINT_TAG = "Spawnpoint";
    private const string ANSWER_TAG = "Answer";

    public GameObject player;
    public Text questionText;
    public Text scoreText;
    public Text multiplierText;
    public Text lifeText;

    public GameObject correctPanel;
    private bool isCorrectPanelActive;
    public Text hintText;
    public GameObject incorrectPanel;
    private bool isIncorrectPanelActive;

    public GameObject correctAudio;
    public GameObject incorrectAudio;

    private Score score;

    private GameObject nextSpawnpoint;
    private GameObject prevSpawnpoint;

    private QuestionGenerator questionGenerator;

    private Question question;
    private GameObject[] answers;

    private int lifeCount;

    void Start()
    {
        Difficulty difficulty = DataPersistence.Settings.GetDifficulty();
        questionGenerator = new QuestionGenerator(DataPersistence.Settings.GetAgeGroup(), difficulty);

        switch (difficulty)
        {
            case Difficulty.EASY:
                score = new Score(player.transform, scoreText, multiplierText, 1);
                break;
            case Difficulty.MEDIUM:
                score = new Score(player.transform, scoreText, multiplierText, 2);
                break;
            default:
                score = new Score(player.transform, scoreText, multiplierText, 3);
                break;
        }

        lifeCount = 5;
        lifeText.text = lifeCount + "x";

        isCorrectPanelActive = isIncorrectPanelActive = false;

        correctAudio.SetActive(false);
        incorrectAudio.SetActive(false);
    }

    void Update()
    {
        correctPanel.SetActive(isCorrectPanelActive);
        incorrectPanel.SetActive(isIncorrectPanelActive);

        correctAudio.SetActive(isCorrectPanelActive);
        incorrectAudio.SetActive(isIncorrectPanelActive);

        score.UpdateScore();

        DataPersistence.SetHighScore(score.GetScore());

        if ((nextSpawnpoint = FindNextSpawnpoint()) != prevSpawnpoint)
        {
            prevSpawnpoint = nextSpawnpoint;
            GameObject ago = Instantiate((GameObject)Resources.Load("Prefabs/Answer"), nextSpawnpoint.transform.position, Quaternion.identity);
            
            answers = FindGameObjectsWithTag(ago, ANSWER_TAG);
            question = questionGenerator.Generate();

            for (int i = 0; i < answers.Length; i++)
                answers[i].transform.Find("text").GetComponent<TextMesh>().text = question.answers[i].Item1.ToString();

            questionText.text = question.question;
        }
    }

    private async void ShowCorrectPanel()
    {
        isIncorrectPanelActive = false;
        isCorrectPanelActive = true;
        await Task.Delay(TimeSpan.FromSeconds(3));
        isCorrectPanelActive = false;
    }

    private async void ShowIncorrectPanel(string message)
    {
        isCorrectPanelActive = false;
        hintText.text = message;
        isIncorrectPanelActive = true;
        await Task.Delay(TimeSpan.FromSeconds(3));
        isIncorrectPanelActive = false;
    }

    public void AnswerCallback(GameObject gameObject)
    {
        float correctAnswer = 0;

        foreach (Tuple<float, bool> ans in question.answers)
        {
            if (ans.Item2)
            {
                correctAnswer = ans.Item1;
                break;
            }
        }

        if (gameObject == null)
        {
            --lifeCount;
            ShowIncorrectPanel(question.question + " = " + correctAnswer);
            return;
        }

        float answer = float.Parse(answers[int.Parse(gameObject.name)].transform.Find("text").GetComponent<TextMesh>().text); ;

        if (answer != correctAnswer)
        {
            --lifeCount;
            ShowIncorrectPanel(question.question + " = " + correctAnswer);
        }
        else
        {
            ShowCorrectPanel();
        }
            
        Destroy(gameObject);

        lifeText.text = lifeCount + "x";
    }

    private GameObject FindNextSpawnpoint()
    {
        GameObject bestTarget = null;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = player.transform.position;
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(SPAWNPOINT_TAG);

        foreach (GameObject potentialTarget in gameObjects)
        {
            if (potentialTarget.transform.position.z < currentPosition.z)
                continue;

            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    private GameObject[] FindGameObjectsWithTag(GameObject parent, string tag)
    {
        List<GameObject> gameObjects = new List<GameObject>();

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);

            if (child.tag == tag)
                gameObjects.Add(child.gameObject);
        }

        return gameObjects.ToArray();
    }
}
