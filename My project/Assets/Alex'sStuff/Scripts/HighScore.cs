using UnityEngine;

public class HighScore
{
    public string HighScoreName { get; private set; }
    public int HighScoreValue { get; private set; }

    public HighScore(string highScoreName, int highScoreValue)
    {
        this.HighScoreName = highScoreName;
        this.HighScoreValue = highScoreValue;
    }
}
