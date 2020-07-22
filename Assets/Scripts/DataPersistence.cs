using UnityEngine;

public static class DataPersistence
{ 
    public static class Settings
    {
        public static void SetAgeGroup(AgeGroup ageGroup)
        {
            PlayerPrefs.SetInt("ageGroup", (int)ageGroup);
            PlayerPrefs.Save();
        }

        public static AgeGroup GetAgeGroup()
        {
            return (AgeGroup)PlayerPrefs.GetInt("ageGroup", 0);
        }

        public static void SetDifficulty(Difficulty difficulty)
        {
            PlayerPrefs.SetInt("difficulty", (int)difficulty);
            PlayerPrefs.Save();
        }

        public static Difficulty GetDifficulty()
        {
            return (Difficulty)PlayerPrefs.GetInt("difficulty", 0);
        }
    }

    public static void SetHighScore(int highScore)
    {
        PlayerPrefs.SetInt("highscore", highScore);
        PlayerPrefs.Save();
    }

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("highscore", 0);
    }

    public static void SetPreviousScore(int score)
    {
        PlayerPrefs.SetInt("previousScore", score);
        PlayerPrefs.Save();
    }

    public static int GetPreviousScore()
    {
        return PlayerPrefs.GetInt("previousScore", 0);
    }
}
