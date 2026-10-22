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
    private int fines = 0; // Penalty for fines (if applicable)

    /*
    private int level1Score= 0;
    private int level2Score= 0;
    private int level3Score= 0;
    private int level4Score= 0;
    private int level5Score= 0;

    private int level1Fines= 0;
    private int level2Fines= 0;
    private int level3Fines= 0;
    private int level4Fines= 0;
    private int level5Fines= 0;

    */

    // Cumulative score across multiple rounds/reloads
    private static float totalCumulativeScore = 0f;

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

        int wrongRealPaintings = handler != null
    ? handler.GetDestroyedReal()
    : wrongBurns;

        int totalErrors = wrongRealPaintings + missedAIPaintings;

        // 2. Handle Timer
        float timeSpent = gameTimer != null ? gameTimer.GetElapsedTime() : 0f;
        if (gameTimer != null) gameTimer.StopTimer();

        // 3. Formula Calculations
        float clampedTime = Mathf.Clamp(timeSpent, 0f, currentLevel.maxTime);
        float timeBonus = currentLevel.flatSum * 0.15f * (1f - (clampedTime / currentLevel.maxTime));
        float basePlusBonus = currentLevel.flatSum + timeBonus;

        // Total penalty count combines wrong burns + missed AI paintings
        //int totalErrors = wrongBurns + missedAIPaintings;
        float scoreBeforePenalties = basePlusBonus;
        float finalScore = scoreBeforePenalties * Mathf.Pow(0.75f, totalErrors);


        fines = Mathf.Max(
            0,
            Mathf.RoundToInt(scoreBeforePenalties - finalScore)
        );
        // Accumulate into the total lifetime earnings
        totalCumulativeScore += finalScore;

        if (scoreBreakdownText != null)
        {
            scoreBreakdownText.gameObject.SetActive(true);
            scoreBreakdownText.text = $"Total Earned: {totalCumulativeScore:N0} kr\n(+{finalScore:N0} kr this round)";
        }

        Debug.Log($"Level: {currentLevel.levelName} | Time: {timeSpent:F1}s | Wrong Burns: {wrongBurns} | Missed AI: {missedAIPaintings} | Round Score: {finalScore:F2} | Total Cumulative: {totalCumulativeScore:F2}");

        ResetErrors();
        return finalScore;
    }

    // Helper method to automatically find the correct level index from your LevelCounter
    private int GetCurrentLevelIndexFromCounter()
    {
        LevelCounter counter = FindAnyObjectByType<LevelCounter>();
        if (counter != null)
        {
            // LevelCounter starts at 1 for Level 1, but array indices start at 0.
            return Mathf.Clamp(counter.GetCurrantLevel() - 1, 0, levels.Length - 1);
        }
        return 0; // Fallback to first level
    }

    // Public wrapper method so DoorInteraction or LevelChanger can call it easily
    public float SaveCurrentLevelScore()
    {
        int levelIndex = GetCurrentLevelIndexFromCounter();
        return CalculateAndDisplayFinalScore(levelIndex);
    }

    // Temporary: lets you test the score without playing the whole level, using the live LevelCounter index
    [ContextMenu("Test Score")]
    private void TestScore()
    {
        int index = GetCurrentLevelIndexFromCounter();
        CalculateAndDisplayFinalScore(index);
    }

    public float GetTotalCumulativeScore()
    {
        return totalCumulativeScore;
    }

    public float GetFines()
    {
        return fines;
    }

    public int GetTotalErrors()
    {
        return wrongBurns + missedAIPaintings;
    }

 /*   public int FinesPerLevel()
    {
        int Level1Score = (int)SaveCurrentLevelScore();
        return 0;
    }

    public int MoneyPerLevel()
    {
        return 0;
    }*/

    // Test Function: displays the accumulated total amount gained so far
    [ContextMenu("Test Total Score")]
    private void TestTotalScore()
    {
        Debug.Log($"[ScoreHandler] Total Cumulative Earnings: {totalCumulativeScore:N2} kr");

        if (scoreBreakdownText != null)
        {
            scoreBreakdownText.gameObject.SetActive(true);
            scoreBreakdownText.text = $"Total Earned: {totalCumulativeScore:N0} kr";
        }
    }
}