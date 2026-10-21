using UnityEngine;
using TMPro;

public class ScoreHandler : MonoBehaviour
{
    [System.Serializable]
    public struct LevelConfig
    {
        public string levelName;
        public float flatSum;
        public float maxTime;
    }

    [Header("UI Reference")]
    [SerializeField] private TMP_Text scoreBreakdownText;

    [Header("Level Settings")]
    [Tooltip("Index 0 = Level 1, Index 1 = Level 2, etc.")]
    [SerializeField] private LevelConfig[] levels;

    [Header("Timer Reference")]
    [SerializeField] private GameTimer gameTimer;

    private int wrongGuesses = 0;

    public void AddError()
    {
        wrongGuesses++;
    }

    public void ResetErrors()
    {
        wrongGuesses = 0;
    }

    public float CalculateAndDisplayFinalScore(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Length)
        {
            Debug.LogError("Invalid level index sent to ScoreHandler!");
            return 0f;
        }

        LevelConfig currentLevel = levels[levelIndex];

        float timeSpent = gameTimer != null ? gameTimer.GetElapsedTime() : 0f;
        if (gameTimer != null) gameTimer.StopTimer();

        float clampedTime = Mathf.Clamp(timeSpent, 0f, currentLevel.maxTime);
        float timeBonus = currentLevel.flatSum * 0.15f * (1f - (clampedTime / currentLevel.maxTime));
        float basePlusBonus = currentLevel.flatSum + timeBonus;
        float finalScore = basePlusBonus * Mathf.Pow(0.75f, wrongGuesses);

        if (scoreBreakdownText != null)
        {
            scoreBreakdownText.gameObject.SetActive(true);
            scoreBreakdownText.text = $"Score: {finalScore:N0}";
        }

        Debug.Log($"Level: {currentLevel.levelName} | FinalScore: {finalScore:F2}");

        ResetErrors();
        return finalScore;
    }

    // Temporary: lets you test the score without playing the whole level
    [ContextMenu("Test Score")]
    private void TestScore()
    {
        CalculateAndDisplayFinalScore(0);
    }
}