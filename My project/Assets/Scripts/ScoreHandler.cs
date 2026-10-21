
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

    [Header("Game Reference")]
    [Tooltip("Optional fallback. The live GameHandler is looked up at calculation time.")]
    [SerializeField] private GameHandler gameHandler;

    private int wrongBurns = 0; // Penalty for burning a Real painting
    private int missedAIPaintings = 0; // Penalty for missing/leaving AI paintings unburned

    public void AddError()
    {
        wrongBurns++;
    }

    public void AddMissedAI(int count = 1)
    {
        missedAIPaintings += count;
    }

    public void ResetErrors()
    {
        wrongBurns = 0;
        missedAIPaintings = 0;
    }

    public float CalculateAndDisplayFinalScore(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Length)
        {
            Debug.LogError("Invalid level index sent to ScoreHandler!");
            return 0f;
        }

        LevelConfig currentLevel = levels[levelIndex];

        // 1. Calculate unburned / missed AI paintings using the live GameHandler
        GameHandler handler = FindObjectOfType<GameHandler>();
        if (handler == null) handler = gameHandler; // fallback to the Inspector reference

        if (handler != null)
        {
            int totalAI = handler.GetTotalAI();
            int destroyedAI = handler.GetDestroyedAI();
            int unburnedAI = Mathf.Max(0, totalAI - destroyedAI);

            Debug.Log($"TotalAI: {totalAI} | DestroyedAI: {destroyedAI} | UnburnedAI: {unburnedAI}");

            missedAIPaintings = unburnedAI; // assign, don't add
        }

        // 2. Handle Timer
        float timeSpent = gameTimer != null ? gameTimer.GetElapsedTime() : 0f;
        if (gameTimer != null) gameTimer.StopTimer();

        // 3. Formula Calculations
        float clampedTime = Mathf.Clamp(timeSpent, 0f, currentLevel.maxTime);
        float timeBonus = currentLevel.flatSum * 0.15f * (1f - (clampedTime / currentLevel.maxTime));
        float basePlusBonus = currentLevel.flatSum + timeBonus;

        // Total penalty count combines wrong burns + missed AI paintings
        int totalErrors = wrongBurns + missedAIPaintings;
        float finalScore = basePlusBonus * Mathf.Pow(0.75f, totalErrors);

        if (scoreBreakdownText != null)
        {
            scoreBreakdownText.gameObject.SetActive(true);
            scoreBreakdownText.text = $"{finalScore:N0} kr";
        }

        Debug.Log($"Level: {currentLevel.levelName} | Time: {timeSpent:F1}s | Wrong Burns: {wrongBurns} | Missed AI: {missedAIPaintings} | FinalScore: {finalScore:F2}");

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


