using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("UI-koppling")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Inställningar")]
    [SerializeField] private float maxTime = 120f;

    private float remainingTime;
    private bool isRunning = false;

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
            
        }

        
        UpdateUI();
    }

    public void StartTimer()
    {
        remainingTime = maxTime;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

  
    public float GetElapsedTime()
    {
        return maxTime - remainingTime;
    }

    private void UpdateUI()
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(remainingTime);
            timerText.text = $"Time remaining: {seconds}s";
        }
    }
}