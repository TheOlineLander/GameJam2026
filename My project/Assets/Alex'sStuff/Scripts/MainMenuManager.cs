using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject videoPlayerObject;
    [SerializeField] private GameObject videoImage;

    [SerializeField] private GameObject highScorePanel;
    [SerializeField] private GameObject titleObject;
    [SerializeField] private GameObject buttonsPanel;
    [SerializeField] private GameObject difficultyPanel;

    private VideoPlayer videoPlayer;
    private HighScoreDisplay highScoreDisplay;
    
    
    private void Awake()
    {
        videoPlayer = videoPlayerObject.GetComponent<VideoPlayer>();
        highScoreDisplay = highScorePanel.GetComponent<HighScoreDisplay>();
        highScoreDisplay.InitializeDisplay();
    }

    public void HandlePlay() => ShowDifficulty();
    
    public void HandleShowHighScores() => ShowHighScore();

    public void HandleCancel() => Home();

    public void HadleDifficulty(int difficulty) => StartGame(difficulty);
    
    public void HandlePlayUIVideo() => PlayVideo();

    private void StartGame(int difficulty)
    {
        //Start game logic

        Debug.Log("Difficulty index " + difficulty + " was chosen");
    }

    private void ShowHighScore()
    {
        titleObject.SetActive(false);
        buttonsPanel.SetActive(false);

        highScorePanel.SetActive(true);
        highScoreDisplay.ShowHighScores();
    }

    private void ShowDifficulty()
    {
        difficultyPanel.SetActive(true);
        buttonsPanel.SetActive(false);
    }

    private void PlayVideo()
    {
        videoPlayerObject.SetActive(true);
        videoImage.SetActive(true);
        videoPlayer.Play();
    }

    private void Home()
    {
        difficultyPanel.SetActive(false);
        highScorePanel.SetActive(false);

        buttonsPanel.SetActive(true);
        titleObject.SetActive(true);
    }
}
