using System;

public class Question
{
    public const short ARRAY_LENGTH = 3;

    public string question { get; set; }
    public Tuple<float, bool>[] answers { get; set; }

    public Question()
    {
        answers = new Tuple<float, bool>[ARRAY_LENGTH];
    }

    public Tuple<float, bool>[] GetRoundedAnswers()
    {
        Tuple<float, bool>[] newAnswers = new Tuple<float, bool>[ARRAY_LENGTH];

        for (int i = 0; i < ARRAY_LENGTH; i++)
            newAnswers[i] = new Tuple<float, bool>((float)Math.Round(answers[i].Item1, 2), answers[i].Item2);

        return newAnswers;
    }
}
