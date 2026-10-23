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
    [SerializeField] private TextMeshProUGUI PaycheckText;


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
    [SerializeField] private TextMeshProUGUI lastTaskText;

    [Header("Timing")]
    [SerializeField] private float textFadeInDuration = 1f;
    [SerializeField] private float textFadeOutDuration = 1f;
    [SerializeField] private float canvasFadeOutDuration = 1f;

    [Header("Scene Fade")]
    private SceneFade sceneFade;


    private Coroutine dialogueCoroutine;

    private void Awake()
    {
        // Vi letar efter referenser i Awake i stället för Start 
        // för att säkerställa att de finns redo innan andra script anropar dem.
        sceneFade = FindObjectOfType<SceneFade>();
        scoreHandler = FindObjectOfType<ScoreHandler>();
        levelcounter = FindAnyObjectByType<LevelCounter>();
    }

    private void Start()
    {
        if (levelcounter == null)
        {
            Debug.LogError("DialogueSequence: LevelCounter saknas i scenen!");
            return;
        }

        if (levelcounter.GetCurrantLevel() < 2)
        {
            ShowDialogue(introMessages);
        }
        else if (levelcounter.GetCurrantLevel() <= 5)
        {
            canvasGroup.alpha = 0f;
            if (gameTimer != null) gameTimer.StartTimer();
        }
        else if (levelcounter.GetCurrantLevel() == 6)
        {
            Debug.Log("Level 6 reached. Showing last message.");
            ShowLastMessage();
            StartCoroutine(WaitAndGoToMainMenu());
        }
    }


    public void ShowIntroDialogue()
    {
        ShowDialogue(introMessages);
        Debug.Log("Intro dialogue shown.");
    }

    public void ShowSuccessfullMessage()
    {
        ShowDialogue(GetSuccessfulTaskMessages());
    }

    public void ShowFaliedTaskMessage()
    {
        ShowDialogue(GetFailedTaskMessages());
    }

    public void ShowLastMessage()
    {
        Debug.Log("Showing last message.");
        ShowDialogue(GetLastText());
    }

    private string[] GetSuccessfulTaskMessages()
    {
        string CongratulationsMessage = $"<color=green>Congratulations!\n</color>";
        return new string[]
        {
            CongratulationsMessage + "You managed to find all AI generated paintings!\n\n" + ShowPaycheckText()
        };
    }

    private string[] GetFailedTaskMessages()
    {
        string Failed = $"<color=red>failed</color>";
        string fired = $"<color=red>YOU ARE FIRED!\n</color>";
        return new string[]
        {
            "You " + Failed + " to destroy all the AI generated paintings!\n\n " + fired + "\n\n"+ ShowPaycheckText()
        };
    }

    private string ShowPaycheckText()
    {
        if (scoreHandler == null) return "Score data unavailable.";

        string paycheck = $"<color=green>Your expected paycheck: {scoreHandler.GetTotalCumulativeScore():N0} MONEY.</color>";
        string fines = $"<color=red>Your fines: {scoreHandler.GetFines():N0} MONEY.</color>";

        return paycheck + "\n" + fines;
    }

    private string[] GetLastText()
    {
        print("TotalScore: " + scoreHandler.GetTotalCumulativeScore());
        // SÄKERHETSKONTROLL: Om scoreHandler är null, kraschar inte spelet.
        if (scoreHandler == null)
        {
            Debug.LogError("DialogueSequence: scoreHandler är null i GetLastText!");
            return new string[] { "The week is over, but your score data could not be loaded." };
        }

        string lastText = $"The week is over and your total earnings are:\n" +
            $"<color=green>Your total earned: {scoreHandler.GetTotalCumulativeScore():N0} MONEY.</color>" +
            $"\n<color=red>Your total fines: {scoreHandler.GetFines():N0} MONEY.</color>";

        return new string[] { lastText };
    }


    private void ShowDialogue(string[] messages)
    {
        
        if (gameTimer != null) gameTimer.ResetTimer();

        if (messages == null || messages.Length == 0)
        {
            return;
        }

        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }

        gameObject.SetActive(true);
        if (sceneFade != null) sceneFade.ShowFullFade();

        if (canvasGroup != null) canvasGroup.alpha = 1f;
        if (dialogueText != null) dialogueText.alpha = 0f;

        SetPlayerControl(false);

        dialogueCoroutine = StartCoroutine(PlayDialogueSequence(messages));
    }

    private IEnumerator PlayDialogueSequence(string[] messages)
    {
        Debug.Log("Dialogue sequence started.");
        for (int i = 0; i < messages.Length; i++)
        {
            if (dialogueText != null) dialogueText.text = messages[i];

            yield return StartCoroutine(FadeText(0f, 1f, textFadeInDuration));

            // Vänta på Space eller högerklick
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

        dialogueCoroutine = null;
        gameObject.SetActive(false);
        Debug.Log("Dialogue sequence ended.");
    }

    private bool IsNextInputPressed()
    {
        bool spacePressed = Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame;
        bool rightMouseButtonPressed = Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame;

        return spacePressed || rightMouseButtonPressed;
    }

    private void SetPlayerControl(bool canControlPlayer)
    {
        if (playerMovement != null)
        {
            playerMovement.SetGameplayFOV();
            if (freezePlayerDuringDialogue)
            {
                playerMovement.enabled = canControlPlayer;
            }
        }

        if (canControlPlayer && gameTimer != null)
        {
            gameTimer.StartTimer();
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private IEnumerator FadeText(float startAlpha, float targetAlpha, float duration)
    {
        if (dialogueText == null) yield break;
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
        if (canvasGroup == null) yield break;
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

    private IEnumerator WaitAndGoToMainMenu()
    {
        yield return new WaitForSeconds(0.2f);

        // Vänta tills dialogen har spelats klart helt och hållet
        yield return new WaitUntil(() => !IsDialoguePlaying);

        Debug.Log("End of game. Loading Main Menu...");
        if (levelcounter != null)
        {
            Destroy(levelcounter.gameObject);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (sceneFade != null)
        {
            sceneFade.LoadScene("MainMenu");
        }
    }

    // Fixade den avklippta propertyn här:
    public bool IsDialoguePlaying => dialogueCoroutine != null;
}
