using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Gameplay : MonoBehaviour
{
    private const string LIFE_SPAWNPOINT_TAG = "LifeSpawnpoint";
    private const string SPAWNPOINT_TAG = "Spawnpoint";
    private const string ANSWER_TAG = "Answer";

    public GameObject player;
    public Text questionText;
    public Text scoreText;
    public Text multiplierText;
    public Text lifeText;
    public Text highscoreText;

    public GameObject correctPanel;
    private bool isCorrectPanelActive;
    public Text hintText;
    public GameObject incorrectPanel;
    private bool isIncorrectPanelActive;

    public GameObject correctAudio;
    public bool isCorrectAudioActive;
    public GameObject incorrectAudio;
    public bool isIncorrectAudioActive;
    public GameObject bgm;

    private Score score;

    private GameObject nextSpawnpoint;
    private GameObject prevSpawnpoint;

    private GameObject nextLifeSpawnpoint;
    private GameObject prevLifeSpawnpoint;

    private QuestionGenerator questionGenerator;

    private Question question;
    private GameObject[] answers;

    private int lifeCount;
    private int currentHighscore;
    private bool isMuted;

    private System.Random random;

    public Gameplay()
    {
        random = new System.Random(DateTime.Now.Second);
    }

    void Start()
    {
        isMuted = DataPersistence.GetMute();

        Difficulty difficulty = DataPersistence.Settings.GetDifficulty();
        questionGenerator = new QuestionGenerator(DataPersistence.Settings.GetAgeGroup(), difficulty);

        currentHighscore = DataPersistence.GetHighScore();

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
        isCorrectAudioActive = isIncorrectAudioActive = false;

        bgm.SetActive(!isMuted);
        correctAudio.SetActive(false);
        incorrectAudio.SetActive(false);
    }

    void Update()
    {
        bool sound = DataPersistence.GetMute();

        if (isMuted != sound)
        {
            isMuted = sound;
            bgm.SetActive(!isMuted);
        }

        correctPanel.SetActive(isCorrectPanelActive);
        incorrectPanel.SetActive(isIncorrectPanelActive);

        correctAudio.SetActive(isCorrectAudioActive);
        incorrectAudio.SetActive(isIncorrectAudioActive);

        score.UpdateScore();

        if (score.GetScore() > currentHighscore)
            highscoreText.text = "Highscore: " + score.GetScore();

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

        if ((nextLifeSpawnpoint = FindNextLifeSpawnpoint()) != prevLifeSpawnpoint)
        {
            prevLifeSpawnpoint = nextLifeSpawnpoint;

            // Add the life, and collider logic will do
            GameObject life = new GameObject("Life");

            if (random.NextDouble() > 0.9)
            {
                SpriteRenderer renderer = life.AddComponent<SpriteRenderer>();
                renderer.sprite = Resources.Load<Sprite>("Images/love shape");
                life.transform.position = nextLifeSpawnpoint.transform.position;
                life.tag = "Life";
                BoxCollider boxCollider = life.AddComponent<BoxCollider>();
                boxCollider.isTrigger = true;
            }
        }
    }

    private async void ShowCorrectPanel()
    {
        isIncorrectAudioActive = false;
        isIncorrectPanelActive = false;
        isCorrectAudioActive = !isMuted;
        isCorrectPanelActive = true;
        await Task.Delay(TimeSpan.FromSeconds(2));
        isCorrectPanelActive = false;
        isCorrectAudioActive = false;
    }

    private async void ShowIncorrectPanel(string message)
    {
        isCorrectAudioActive = false;
        isCorrectPanelActive = false;
        hintText.text = message;
        isIncorrectPanelActive = true;
        isIncorrectAudioActive = !isMuted;
        await Task.Delay(TimeSpan.FromSeconds(2));
        isIncorrectPanelActive = false;
        isIncorrectAudioActive = false;
    }

    public async void AddLifeCallback(GameObject gameObject)
    {
        Destroy(gameObject);
        lifeText.text = ++lifeCount + "x";
        isIncorrectAudioActive = false;
        isCorrectAudioActive = !isMuted;
        await Task.Delay(TimeSpan.FromSeconds(2));
        isCorrectAudioActive = false;
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
            score.ResetMultiplier();
            --lifeCount;
            ShowIncorrectPanel(question.question + " = " + correctAnswer);
            return;
        }

        float answer = float.Parse(answers[int.Parse(gameObject.name)].transform.Find("text").GetComponent<TextMesh>().text); ;

        if (answer != correctAnswer)
        {
            score.ResetMultiplier();
            --lifeCount;
            ShowIncorrectPanel(question.question + " = " + correctAnswer);
        }
        else
        {
            score.IncrementMultiplier();
            ShowCorrectPanel();
        }
            
        Destroy(gameObject);

        lifeText.text = lifeCount + "x";

        if (lifeCount <= 0)
        {
            UpdateHighscore();
            DataPersistence.SetPreviousScore(score.GetScore());
            SceneManager.LoadScene("GameOver");
        }
    }

    public void UpdateHighscore()
    {
        if (score.GetScore() > DataPersistence.GetHighScore())
            DataPersistence.SetHighScore(score.GetScore());
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

    private GameObject FindNextLifeSpawnpoint()
    {
        GameObject bestTarget = null;

        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = player.transform.position;
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(LIFE_SPAWNPOINT_TAG);

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
}
