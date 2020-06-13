using System;

public class Question
{
    public const short ARRAY_LENGTH = 3;

    public string question { get; set; }
    public float[] answers { get; set; }

    public Question()
    {
        answers = new float[ARRAY_LENGTH];
    }

    public float[] GetRoundedAnswers()
    {
        float[] newAnswers = new float[ARRAY_LENGTH];

        for (int i = 0; i < ARRAY_LENGTH; i++)
            newAnswers[i] = (float) Math.Round(answers[i], 2);

        return newAnswers;
    }
}
