using UnityEngine;
using TMPro;
using UnityEngine.InputSystem; // VIKTIGT: Krävs för nya Input System

public class DebugScoreTester : MonoBehaviour
{
    [Header("Kopplingar")]
    [SerializeField] private ScoreHandler scoreHandler;
    [SerializeField] private GameTimer gameTimer;

    [Header("UI för Debug-resultat")]
    [SerializeField] private TextMeshProUGUI debugResultText;

    [Header("Testinställningar")]
    [SerializeField] private int testLevelIndex = 0;
    [SerializeField] private int testWrongGuesses = 1;

    void Update()
    {
        // Kontrollera om nya Input System är tillgängligt och om Space trycks ner
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            RunDebugScoreTest();
        }
    }

    private void RunDebugScoreTest()
    {
        if (scoreHandler == null || gameTimer == null)
        {
            Debug.LogWarning("ScoreHandler eller GameTimer är inte kopplad i Inspector!");
            return;
        }

        gameTimer.StopTimer();
        float timeSpent = gameTimer.GetElapsedTime();
        float finalScore = scoreHandler.CalculateFinalScore(testLevelIndex, timeSpent, testWrongGuesses);

        Debug.Log($"--- DEBUG TEST KÖRD --- \n" +
                  $"Tid förbrukad: {timeSpent:F2}s \n" +
                  $"Felaktiga gissningar: {testWrongGuesses} \n" +
                  $"Slutgiltig summa: {finalScore:F2} kr");

        if (debugResultText != null)
        {
            debugResultText.text = $"Slutsumma: {Mathf.RoundToInt(finalScore)} kr";
        }
    }
}