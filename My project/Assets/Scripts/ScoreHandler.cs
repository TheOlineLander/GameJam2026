using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [System.Serializable]
    public struct LevelConfig
    {
        public string levelName;
        public float flatSum;   // T.ex. 10000, 20000, 30000
        public float maxTime;   // Maxtid i sekunder för denna nivå
    }

    [Header("Nivåinställningar")]
    [Tooltip("Index 0 = Nivå 1, Index 1 = Nivå 2, osv.")]
    [SerializeField] private LevelConfig[] levels;

    public float CalculateFinalScore(int levelIndex, float timeSpent, int wrongGuesses)
    {
        
        if (levelIndex < 0 || levelIndex >= levels.Length)
        {
            Debug.LogError("Ogiltigt nivåindex skickat till ScoreHandler!");
            return 0f;
        }

        LevelConfig currentLevel = levels[levelIndex];

        // 2. Säkerställ att tiden inte understiger 0 eller överstiger maxtid
        float clampedTime = Mathf.Clamp(timeSpent, 0f, currentLevel.maxTime);

        // 3. Beräkna tidsbonus: Flatsumma * 0.15 * (1 - TidFörbrukad / MaxTid)
        float timeBonus = currentLevel.flatSum * 0.15f * (1f - (clampedTime / currentLevel.maxTime));

        // 4. Lägg till flatsumma + tidsbonus
        float basePlusBonus = currentLevel.flatSum + timeBonus;

        // 5. Applicera straffet för felaktiga gissningar: (0.75)^AntalFel (eftersom 25% dras av per fel)
        float finalScore = basePlusBonus * Mathf.Pow(0.75f, wrongGuesses);

        // Debug-logg för att enklare verifiera uträkningen under speltestning
        Debug.Log($"Nivå: {currentLevel.levelName} | Flatsumma: {currentLevel.flatSum} | " +
                  $"Tidsbonus: {timeBonus:F2} | Felaktiga gissningar: {wrongGuesses} | " +
                  $"Slutpoäng: {finalScore:F2}");

        return finalScore;
    }
}