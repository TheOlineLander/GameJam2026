using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MainMenuManager : MonoBehaviour
{
    [Header("MainUIComponents")]
    [SerializeField] private GameObject titleObject;
    [SerializeField] private GameObject buttonsPanel;

    [Header("HighScore")]
    [SerializeField] private GameObject highScorePanel;

    [Header("Difficulty")]
    [SerializeField] private GameObject difficultyPanel;

    [Header("Video")]
    [SerializeField] private GameObject videoPlayerObject;
    [SerializeField] private GameObject videoImage;

    [Header("Utility")]
    [SerializeField] private GameObject confirmationPanel;

    private VideoPlayer videoPlayer;
    private HighScoreDisplay highScoreDisplay;
    
    private void Awake()
    {
        videoPlayer = videoPlayerObject.GetComponent<VideoPlayer>();
        highScoreDisplay = highScorePanel.GetComponent<HighScoreDisplay>();
        highScoreDisplay.InitializeDisplay();
    }

    #region Button Handlers

    public void HandlePlay() => ShowDifficulty();

    public void HandleExit() => ShowConfirmationPanel();

    public void HandleConfirmation() => ExitGame();
    
    public void HandleShowHighScores() => ShowHighScore();

    public void HandleCancel() => Home();

    public void HadleDifficulty(int difficulty) => StartGame(difficulty);
    
    public void HandlePlayUIVideo() => PlayVideo();

    #endregion

    #region Button Logic

    private void StartGame(int difficulty)
    {
        //Start game logic

        Debug.Log("Difficulty index " + difficulty + " was chosen");
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void ShowConfirmationPanel()
    {
        confirmationPanel.SetActive(true);
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
        confirmationPanel.SetActive(false);

        buttonsPanel.SetActive(true);
        titleObject.SetActive(true);
    }

    #endregion
}
