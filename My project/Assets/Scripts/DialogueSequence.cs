using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueSequence : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI dialogueText;
    private LevelCounter levelcounter;
    private ScoreHandler scoreHandler;
    [SerializeField] private    TextMeshProUGUI PaycheckText;


    [Header("Player Control")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private bool freezePlayerDuringDialogue = true;
    [SerializeField] private GameTimer gameTimer;

    [Header("Dialogue Groups")]
    [TextArea(2, 5)]
    [SerializeField] private string[] introMessages;


    [Header("Task Result Text")]
    [SerializeField] private TextMeshProUGUI successfulTaskText;
    [SerializeField] private TextMeshProUGUI failedTaskText;

    [Header("Timing")]
    [SerializeField] private float textFadeInDuration = 1f;
    [SerializeField] private float textFadeOutDuration = 1f;
    [SerializeField] private float canvasFadeOutDuration = 1f;

    [Header("Scene Fade")]
    private SceneFade sceneFade;


    private Coroutine dialogueCoroutine;

    private void Start()
    {
        sceneFade = FindObjectOfType<SceneFade>();
        scoreHandler = FindObjectOfType<ScoreHandler>();
        levelcounter = FindAnyObjectByType<LevelCounter>();

        if (levelcounter.GetCurrantLevel()<2)
        {
            ShowDialogue(introMessages);
        }
        else { 
            canvasGroup.alpha = 0f;
            gameTimer.StartTimer();
        }
        
    }

    public void ShowIntroDialogue()
    {
        ShowDialogue(introMessages);
        Debug.Log("Intro dialogue shown.");
    }

    public void ShowSuccessfullMessage()
    {
        ShowPaycheckText();
        ShowDialogue(GetSuccessfulTaskMessages());
    }

    public void ShowFaliedTaskMessage()
    {
        ShowPaycheckText();
        ShowDialogue(GetFailedTaskMessages());
    }

    private string [] GetSuccessfulTaskMessages()
    {
        return new string[]
        {
            "Congratulations!\n You manage to find all AI generated paintings!\n\n" + ShowPaycheckText()
        };
    }

    private string [] GetFailedTaskMessages()
    {
        return new string[]
        {
            "You failed to destroy all the AI generated paintings!\n You are FIRED!\n\n"+ ShowPaycheckText()
        };
    }

    private string ShowPaycheckText()
    {
        string paycheck =
        $"<color=green>Your paycheck: {scoreHandler.GetTotalCumulativeScore():N0} SEK.</color>";

        string fines =
            $"<color=red>Your fines: {scoreHandler.GetFines():N0} SEK.</color>";

        return paycheck + "\n" + fines;
    }

    private void ShowDialogue(string[] messages)
    {
        gameTimer.ResetTimer(); //TIMER MAYBE MOVE

        if (messages == null || messages.Length == 0)
        {
            return;
        }

        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }

        gameObject.SetActive(true);
        sceneFade.ShowFullFade();
        canvasGroup.alpha = 1f;
        dialogueText.alpha = 0f;

        SetPlayerControl(false);

        dialogueCoroutine = StartCoroutine(PlayDialogueSequence(messages));
    }

    private IEnumerator PlayDialogueSequence(string[] messages)
    {
        Debug.Log("Dialogue sequence started.");
        for (int i = 0; i < messages.Length; i++)
        {
            dialogueText.text = messages[i];

            yield return StartCoroutine(FadeText(0f, 1f, textFadeInDuration));

            // Wait for Space or Right Mouse Button.
            yield return new WaitUntil(IsNextInputPressed);

            bool isLastMessage = i == messages.Length - 1;

            if (!isLastMessage)
            {
                yield return StartCoroutine(FadeText(1f, 0f, textFadeOutDuration));
            }
        }
        Debug.Log("Dialogue starts fading out.");
        SetPlayerControl(true);
        yield return StartCoroutine(FadeCanvas(1f, 0f, canvasFadeOutDuration));
        /*if (levelcounter.GetCurrantLevel() < 2)
        {
           
            Debug.Log("Start coroutine");
        }
        else
        {
            canvasGroup.alpha = 0f;
            Debug.Log("Alpha is 0");
        }*/


      
        dialogueCoroutine = null;
        gameObject.SetActive(false);
        Debug.Log("Dialogue sequence ended.");
    }

    private bool IsNextInputPressed()
    {
        bool spacePressed =
            Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame;

        bool rightMouseButtonPressed =
            Mouse.current != null &&
            Mouse.current.rightButton.wasPressedThisFrame;

        return spacePressed || rightMouseButtonPressed;
    }

    private void SetPlayerControl(bool canControlPlayer)
    {
        playerMovement.SetGameplayFOV();

        if (freezePlayerDuringDialogue)
        {
            playerMovement.enabled = canControlPlayer;
        }
        //timer
        if (canControlPlayer && gameTimer != null)
        {
            gameTimer.StartTimer();
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("Player can move");
    }

    private IEnumerator FadeText(float startAlpha, float targetAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / duration);
            dialogueText.alpha = Mathf.Lerp(startAlpha, targetAlpha, progress);

            yield return null;
        }

        dialogueText.alpha = targetAlpha;
    }

    private IEnumerator FadeCanvas(float startAlpha, float targetAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / duration);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, progress);

            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }

    public bool IsDialoguePlaying => dialogueCoroutine != null;
}