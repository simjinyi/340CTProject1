using System;
using System.Linq;
using System.Net;

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
    private Difficulty difficulty;
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

        Operator op = GenerateOperator();
        Tuple<float, float> operands = GenerateOperands(op);
        Tuple<float, bool>[] answers = GenerateAnswers(op, operands);

        question.question = operands.Item1.ToString() + " " + OperatorToString(op) + " " + operands.Item2.ToString();
        question.answers = answers;

        return question;
    }

    private Operator GenerateOperator()
    {
        if (ageGroup == AgeGroup._6TO8)
            return (Operator)random.Next(2);

        return (Operator)random.Next(4);
    }

    private Tuple<float, float> GenerateOperands(Operator op)
    {
        short maxNum = 10;

        switch (ageGroup)
        {
            case AgeGroup._6TO8:
                if (difficulty == Difficulty.HARD)
                    maxNum = 40;
                break;

            case AgeGroup._9TO10:
                if (difficulty != Difficulty.EASY && op == Operator.MUL)
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

        short[] operands = { (short)random.Next(maxNum), (short)(random.Next(maxNum) + 1) };

        if (op == Operator.SUB || op == Operator.DIV)
        {
            short largerIndex = operands[0] >= operands[1] ? operands[0] : operands[1];
            short smallerIndex = (short)(operands.Length - largerIndex - 1);

            if (op == Operator.DIV)
            {
                if (operands[smallerIndex] <= 0)
                    operands[smallerIndex] += 1;

                operands[smallerIndex] += (short)(operands[largerIndex] % operands[smallerIndex]);
            }

            return new Tuple<float, float>(operands[largerIndex], operands[smallerIndex]);
        }

        return new Tuple<float, float>(operands[0], operands[1]);
    }

    private Tuple<float, bool>[] GenerateAnswers(Operator op, Tuple<float, float> operands)
    {
        float correctAnswer = GetCalculator(op)(operands.Item1, operands.Item2);

        if (difficulty == Difficulty.EASY)
            return (new Tuple<float, bool>[] { new Tuple<float, bool>(correctAnswer - 1, false), new Tuple<float, bool>(correctAnswer, true), new Tuple<float, bool>(correctAnswer + 1, false) }).OrderBy(x => Guid.NewGuid()).ToArray();

        Tuple<float, bool>[] correctAnswers = new Tuple<float, bool>[3] { new Tuple<float, bool>(correctAnswer, true), null, null };

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
                    correctAnswers[2] = new Tuple<float, bool>(correctAnswer - (random.Next(9) + 1), false);
                }
                else
                {
                    correctAnswers[1] = new Tuple<float, bool>(correctAnswer - 10, false);
                    correctAnswers[2] = new Tuple<float, bool>(correctAnswer - (random.Next(9) + 1), false);
                }
                break;

            default:
                correctAnswers[1] = new Tuple<float, bool>(correctAnswer + 10, false);
                correctAnswers[2] = new Tuple<float, bool>((correctAnswer - 10 < 0) ? (correctAnswer - (random.Next(9) + 1)) : correctAnswer - 10, false);
                break;
        }

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
        return x + y;
    }

    private static float Mul(float x, float y)
    {
        return x + y;
    }

    private static float Div(float x, float y)
    {
        return x + y;
    }

    private string OperatorToString(Operator op)
    {
        string[] strings = { "+", "-", "*", "/" };
        return strings[(int)op];
    }
}
