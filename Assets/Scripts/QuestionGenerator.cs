using System;

public enum AgeGroup 
{ 
    _6TO8, _9TO10, _11TO12 
}

public enum Difficulty 
{ 
    EASY, MEDIUM, HARD 
}

public class QuestionGenerator
{
    private AgeGroup ageGroup;
    private Difficulty difficulty;

    private enum Operator
    {
        ADD, SUB, MUL, DIV
    }

    public QuestionGenerator(AgeGroup ageGroup, Difficulty difficulty)
    {
        this.ageGroup = ageGroup;
        this.difficulty = difficulty;
    }

    public Question Generate()
    {
        Question question = new Question();
        return question;
    }

    private Operator GenerateOperator()
    {
        return (Operator) new Random().Next(3);
    }

    private Tuple<int, int> GenerateOperands(Operator op)
    {
        return null;
    }
}
