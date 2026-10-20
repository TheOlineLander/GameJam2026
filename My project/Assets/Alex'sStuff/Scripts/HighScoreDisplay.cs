using UnityEngine;
using System.Collections.Generic;

public class HighScoreDisplay : MonoBehaviour
{
    [SerializeField] private GameObject highScoreSlotPrefab;
    [SerializeField] private GameObject display;

    [SerializeField] private int maxNumberOfHighScores = 5;

    [SerializeField] private int highScoreExampleOne = 123;
    [SerializeField] private string highScoreExampleNameOne = "Anders";

    [SerializeField] private int highScoreExampleTwo = 897661;
    [SerializeField] private string highScoreExampleNameTwo = "Nils";

    [SerializeField] private int highScoreExampleThree = 50;
    [SerializeField] private string highScoreExampleNameThree = "Perre";

    [SerializeField] private int highScoreExampleFour = 19047;
    [SerializeField] private string highScoreExampleNameFour = "Gurra Sexton";

    [SerializeField] private int highScoreExampleFive = 1990;
    [SerializeField] private string highScoreExampleNameFive = "Sten";


    [SerializeField] private int newHighScoreExampleOne = 2;
    [SerializeField] private string newHighScoreExampleNameOne = "Pelle";

    [SerializeField] private int newHighScoreExampleTwo = 1000000;
    [SerializeField] private string newHighScoreExampleNameTwo = "Bosse";

    private List<HighScore> highScores = new();
    private List<GameObject> highScoreSlots = new();

    public void InitializeDisplay()
    {
        highScores.Clear();
        highScoreSlots.Clear();

        for (int i = 0; i < maxNumberOfHighScores; i++)
        {
            GameObject newHighScoreSlot = Instantiate(highScoreSlotPrefab, display.transform);
            highScoreSlots.Add(newHighScoreSlot);
            newHighScoreSlot.SetActive(false);
        }
    }

    public List<HighScore> GetHighScores()
    {
        List<HighScore> highScoresCopy = new(highScores);
        return highScoresCopy;
    }

    public void TestHighScoreOne()
    {
        CheckForNewHighScore(highScoreExampleNameOne, highScoreExampleOne);
    }

    public void TestHighScoreTwo()
    {
        CheckForNewHighScore(highScoreExampleNameTwo, highScoreExampleTwo);
    }

    public void TestHighScoreThree()
    {
        CheckForNewHighScore(highScoreExampleNameThree, highScoreExampleThree);
    }

    public void TestHighScoreFour()
    {
        CheckForNewHighScore(highScoreExampleNameFour, highScoreExampleFour);
    }

    public void TestHighScoreFive()
    {
        CheckForNewHighScore(highScoreExampleNameFive, highScoreExampleFive);
    }

    public void TestNewHighScoreOne()
    {
        CheckForNewHighScore(newHighScoreExampleNameOne, newHighScoreExampleOne);
    }

    public void TestNewHighScoreTwo()
    {
        CheckForNewHighScore(newHighScoreExampleNameTwo, newHighScoreExampleTwo);
    }

    public void ShowHighScores()
    {
        UpdateHighScores();
    }

    public bool CheckForNewHighScore(string name, int score)
    {
        if (string.IsNullOrEmpty(name) || score < 0 || highScores.Count > maxNumberOfHighScores)
            return false;

        if (highScores.Count < maxNumberOfHighScores)
        {
            AddHighScore(name, score);
            return true;
        }

        if (highScores.Count == maxNumberOfHighScores)
        {
            for (int i = 0; i < highScores.Count; i++)
            {
                HighScore highScore = highScores[i];
                if (score > highScore.HighScoreValue)
                {
                    AddHighScore(name, score);
                    return true;
                }
            }
        }

        return false;
    }

    private void AddHighScore(string name, int score)
    {
        HighScore newHighScore = new(name, score);

        highScores.Add(newHighScore);
        highScores.Sort((a, b) => b.HighScoreValue.CompareTo(a.HighScoreValue));

        if (highScores.Count > maxNumberOfHighScores)
        {
            highScores.RemoveRange(maxNumberOfHighScores, highScores.Count - maxNumberOfHighScores);
        }

        UpdateHighScores();
    }

    private void UpdateHighScores()
    {
        int numberOfHighScores = Mathf.Min(highScores.Count, highScoreSlots.Count);

        for (int i = 0; i < numberOfHighScores; i++)
        {
            HighScore highScore = highScores[i];
            GameObject highScoreSlot = highScoreSlots[i];
            
            highScoreSlot.SetActive(true);
            highScoreSlot.TryGetComponent(out UISlot uiSlot);

            if (uiSlot != null)
                uiSlot.SetContent(highScore.HighScoreName, highScore.HighScoreValue);
        }

        for (int i = numberOfHighScores; i < highScoreSlots.Count; i++)
        {
            highScoreSlots[i].SetActive(false);
        }
    }
}
