using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("UI-koppling")]
    [SerializeField] private TMP_Text timerText;

    [Header("Inställningar")]
    [SerializeField] private float maxTime = 180f;

    private float remainingTime;
    private bool isRunning = false;

    public event System.Action OnTimeUp;
    public float MaxTime => maxTime;

    void Start()
    {
        StartTimer();
    }

    void Update()
    {
        if (!isRunning) return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            StopTimer();
            OnTimeUp?.Invoke();
        }

        UpdateUI();
    }

    public void StartTimer()
    {
        remainingTime = maxTime;
        isRunning = true;
        UpdateUI();
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetElapsedTime()
    {
        return maxTime - remainingTime;
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }

    // NEW: Added a method to retrieve the max time directly if needed by ScoreHandler
    public float GetMaxTime()
    {
        return maxTime;
    }

    private void UpdateUI()
    {
        if (timerText != null)
        {
            int totalSeconds = Mathf.CeilToInt(remainingTime);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            timerText.text = $"Time remaining: {minutes:00}:{seconds:00}";
        }
    }
}