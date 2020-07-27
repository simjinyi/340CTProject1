using System;
using System.Linq;
using System.Collections.Generic;

public enum AgeGroup 
{ 
    _6TO8, _9TO10, _11TO12 
}

public enum Difficulty 
{ 
    EASY, MEDIUM, HARD 
}

delegate float CalculateAnswer(float x, float y);

public class QuestionGenerator
{
    private AgeGroup ageGroup;
    public Difficulty difficulty { get; set; }
    private Random random;

    private enum Operator
    {
        ADD, SUB, MUL, DIV
    }

    public QuestionGenerator(AgeGroup ageGroup, Difficulty difficulty)
    {
        this.ageGroup = ageGroup;
        this.difficulty = difficulty;
        this.random = new Random();
    }

    public Question Generate()
    {
        Question question = new Question();

        // Generator a random operator and operands
        Operator op = GenerateOperator();
        Tuple<float, float> operands = GenerateOperands(op);
        Tuple<float, bool>[] answers = GenerateAnswers(op, operands);

        // Form the question and answers
        question.question = operands.Item1.ToString() + " " + OperatorToString(op) + " " + operands.Item2.ToString();
        question.answers = answers;

        return question;
    }

private Operator GenerateOperator()
{
    // Do not include multiplication and division for age 6 to 8
    if (ageGroup == AgeGroup._6TO8)
        return (Operator)random.Next(2);

    return (Operator)random.Next(4);
}

private Tuple<float, float> GenerateOperands(Operator op)
{
    // Set the maximum number
    short maxNum = 10;

    // Set the different maximum number for the age group and difficulty
    switch (ageGroup)
    {
        case AgeGroup._6TO8:
            if (difficulty == Difficulty.HARD)
                maxNum = 40;
            break;

        case AgeGroup._9TO10:
            if (difficulty != Difficulty.EASY || op == Operator.MUL)
                maxNum = 10;
            else if (difficulty != Difficulty.EASY)
                maxNum = 100;
            break;

        case AgeGroup._11TO12:
            if (op == Operator.MUL)
                maxNum = 12;
            else
                maxNum = 100;
            break;
    }

    // Generate the 2 operands, one has the possibility to be greater than another
    short[] operands = { (short)random.Next(maxNum), (short)(random.Next(maxNum) + 1) };

    // If the operator is subtraction and division
    if (op == Operator.SUB || op == Operator.DIV)
    {
        // Find out the index of the greater and smaller element
        short largerIndex = (short)(operands[0] >= operands[1] ? 0 : 1);
        short smallerIndex = (short)(largerIndex ^ 1);

        if (largerIndex > 1 || smallerIndex > 1)
            throw new Exception();

        // If the operator is division
        if (op == Operator.DIV)
        {
            List<short> denominators = new List<short>();

            // Choose a list of demoninator that divides the nominator completely
            for (int i = 1; i < 100 && i < operands[largerIndex]; ++i)
                if (operands[largerIndex] % i == 0)
                    denominators.Add((short)i);

            // Choose a random denominator that divides the nominator completely
            operands[smallerIndex] = denominators[random.Next(0, denominators.Count)];
        }

        // Make sure that the tuple is in order of the larger followed by the smaller number
        // This prevents negative answers or answers with floating point
        return new Tuple<float, float>(operands[largerIndex], operands[smallerIndex]);
    }

    // If the operator is multiplication and addition, simply return the generated operands
    return new Tuple<float, float>(operands[0], operands[1]);
}

    private Tuple<float, bool>[] GenerateAnswers(Operator op, Tuple<float, float> operands)
    {
        float correctAnswer = GetCalculator(op)(operands.Item1, operands.Item2);

        // If the difficulty is easy or medium, return the set of answers { correctAnswer - 1, correctAnswer, correctAnswer + 1)
        if (difficulty == Difficulty.EASY || difficulty == Difficulty.MEDIUM)
            return (new Tuple<float, bool>[] { new Tuple<float, bool>((correctAnswer - 1) < 0 ? correctAnswer + 2 : correctAnswer - 1, false), new Tuple<float, bool>(correctAnswer, true), new Tuple<float, bool>(correctAnswer + 1, false) }).OrderBy(x => Guid.NewGuid()).ToArray();

        Tuple<float, bool>[] correctAnswers = new Tuple<float, bool>[3] { new Tuple<float, bool>(correctAnswer, true), null, null };

        // If the difficulty is hard
        // Do not allow the player to get the answer based on the last digit
        switch (op)
        {
            case Operator.ADD:
            case Operator.MUL:
                correctAnswers[1] = new Tuple<float, bool>(correctAnswer + 10, false);
                correctAnswers[2] = new Tuple<float, bool>(correctAnswer - (random.Next(9) + 1), false);
                break;

            case Operator.SUB:
                if (correctAnswer - 10 < 0)
                {
                    correctAnswers[1] = new Tuple<float, bool>(correctAnswer + (random.Next(9) + 1), false);
                    correctAnswers[2] = new Tuple<float, bool>(correctAnswer - (random.Next((int)correctAnswer) + 1), false);
                }
                else
                {
                    correctAnswers[1] = new Tuple<float, bool>(correctAnswer - 10, false);
                    correctAnswers[2] = new Tuple<float, bool>(correctAnswer - (random.Next(9) + 1), false);
                }
                break;

            default:
                correctAnswers[1] = new Tuple<float, bool>(correctAnswer + 10, false);
                correctAnswers[2] = new Tuple<float, bool>((correctAnswer - 10 < 0) ? (correctAnswer + (random.Next(9) + 1)) : correctAnswer - 10, false);
                break;
        }

        // Shuffle the set of answers
        return correctAnswers.OrderBy(x => Guid.NewGuid()).ToArray();
    }

    private CalculateAnswer GetCalculator(Operator op)
    {
        switch (op)
        {
            case Operator.ADD:
                return new CalculateAnswer(Add);
            case Operator.SUB:
                return new CalculateAnswer(Sub);
            case Operator.MUL:
                return new CalculateAnswer(Mul);
            default:
                return new CalculateAnswer(Div);
        }
    }

    private static float Add(float x, float y)
    {
        return x + y;
    }

    private static float Sub(float x, float y)
    {
        return x - y;
    }

    private static float Mul(float x, float y)
    {
        return x * y;
    }

    private static float Div(float x, float y)
    {
        return x / y;
    }

    private string OperatorToString(Operator op)
    {
        string[] strings = { "+", "-", "*", "/" };
        return strings[(int)op];
    }
}
