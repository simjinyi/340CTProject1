using System;
using System.Collections;
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

    private void Awake()
    {
        // Increase the speed of the player by 1 unit every 5 second
        InvokeRepeating("IncrementSpeed", 0, 5.0f);
    }

    private void IncrementSpeed()
    {
        // Call to the player movement script to increment the speed
        player.GetComponent<PlayerMovement>().IncrementSpeed();
    }

    void Start()
    {
        // Get the settings
        isMuted = DataPersistence.GetMute();

        Difficulty difficulty = DataPersistence.Settings.GetDifficulty();
        questionGenerator = new QuestionGenerator(DataPersistence.Settings.GetAgeGroup(), difficulty);

        currentHighscore = DataPersistence.GetHighScore();

        // Set the multiplier based on the difficulty
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

        // Initialize the UI components
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
    // Check if the sound is muted
    bool sound = DataPersistence.GetMute();

    if (isMuted != sound)
    {
        isMuted = sound;
        bgm.SetActive(!isMuted);
    }

    // Disable or enable the sound accordingly
    correctPanel.SetActive(isCorrectPanelActive);
    incorrectPanel.SetActive(isIncorrectPanelActive);

    correctAudio.SetActive(isCorrectAudioActive);
    incorrectAudio.SetActive(isIncorrectAudioActive);

    // Update the score
    score.UpdateScore();

    // Update the highscore if necessary
    if (score.GetScore() > currentHighscore)
        highscoreText.text = "Highscore: " + score.GetScore();

    // Triggers when player goes over a question spawnpoint
    if ((nextSpawnpoint = FindNextSpawnpoint()) != prevSpawnpoint)
    {
        // Instantiate the answer prefabs
        prevSpawnpoint = nextSpawnpoint;
        GameObject ago = Instantiate((GameObject)Resources.Load("Prefabs/Answer"), nextSpawnpoint.transform.position, Quaternion.identity);

        // Generate the questions
        answers = FindGameObjectsWithTag(ago, ANSWER_TAG);
        question = questionGenerator.Generate();

        // Populate the answers into the map
        for (int i = 0; i < answers.Length; i++)
            answers[i].transform.Find("text").GetComponent<TextMesh>().text = question.answers[i].Item1.ToString();

        // Update the text in the game
        questionText.text = question.question;
    }

    // Triggers when player goes over a life spawnpoint
    if ((nextLifeSpawnpoint = FindNextLifeSpawnpoint()) != prevLifeSpawnpoint)
    {
        prevLifeSpawnpoint = nextLifeSpawnpoint;

        // 10% chance of generating a life
        if (random.NextDouble() > 0.9)
        {
            // Instantiate a new life and add it into the game
            GameObject life = new GameObject("Life");
            SpriteRenderer renderer = life.AddComponent<SpriteRenderer>();
            renderer.sprite = Resources.Load<Sprite>("Images/love shape");
            life.transform.position = nextLifeSpawnpoint.transform.position;
            life.tag = "Life";
            BoxCollider boxCollider = life.AddComponent<BoxCollider>();
            boxCollider.isTrigger = true;
        }
    }
}

    private IEnumerator ShowCorrectPanel()
    {
        isIncorrectAudioActive = false;
        isIncorrectPanelActive = false;
        isCorrectAudioActive = !isMuted;
        isCorrectPanelActive = true;
        yield return new WaitForSeconds(2);
        isCorrectPanelActive = false;
        isCorrectAudioActive = false;
    }

    private IEnumerator ShowIncorrectPanel(string message)
    {
        isCorrectAudioActive = false;
        isCorrectPanelActive = false;
        hintText.text = message;
        isIncorrectPanelActive = true;
        isIncorrectAudioActive = !isMuted;
        yield return new WaitForSeconds(2);
        isIncorrectPanelActive = false;
        isIncorrectAudioActive = false;
    }

public IEnumerator AddLifeCallback(GameObject gameObject)
{
    // Destroy the life
    Destroy(gameObject);

    // Add the life
    lifeText.text = ++lifeCount + "x";

    // Play the sound for 2 seconds
    isIncorrectAudioActive = false;
    isCorrectAudioActive = !isMuted;
    yield return new WaitForSeconds(2);
    isCorrectAudioActive = false;
}

public void AnswerCallback(GameObject gameObject)
{
    float correctAnswer = 0;

    // Find the value of the correct answer
    foreach (Tuple<float, bool> ans in question.answers)
    {
        if (ans.Item2)
        {
            correctAnswer = ans.Item1;
            break;
        }
    }

    // If the player did not collide with anything
    if (gameObject == null)
    {
        // Reset the multiplier and decrement the life count
        score.ResetMultiplier();
        --lifeCount;

        // Show the incorrect panel with the message
        StartCoroutine(ShowIncorrectPanel(question.question + " = " + correctAnswer));
        return;
    }

    // Get the answer collided by the player
    float answer = float.Parse(answers[int.Parse(gameObject.name)].transform.Find("text").GetComponent<TextMesh>().text); ;

    // The player answered wrongly
    if (answer != correctAnswer)
    {
        // Reset the multiplier, decrement the life count and show the incorrect panel
        score.ResetMultiplier();
        --lifeCount;
        StartCoroutine(ShowIncorrectPanel(question.question + " = " + correctAnswer));
    }
    else
    {
        // Increase the multiplier and show the correct panel
        score.IncrementMultiplier();
        StartCoroutine(ShowCorrectPanel());
    }
            
    // Remove the collided object
    Destroy(gameObject);

    // Update the life count display
    lifeText.text = lifeCount + "x";

    // End the game if the life count falls to 0
    if (lifeCount <= 0)
    {
        // Update the highscore and load the game over scene
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
