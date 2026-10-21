using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueSequence : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Player Control")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private bool freezePlayerDuringDialogue = true;

    [Header("Dialogue Groups")]
    [TextArea(2, 5)]
    [SerializeField] private string[] introMessages;

    [TextArea(2, 5)]
    [SerializeField] private string[] afterLevel1Messages;

    [TextArea(2, 5)]
    [SerializeField] private string[] afterLevel2Messages;

    [TextArea(2, 5)]
    [SerializeField] private string[] afterLevel3Messages;

    [Header("Timing")]
    [SerializeField] private float textFadeInDuration = 1f;
    [SerializeField] private float textFadeOutDuration = 1f;
    [SerializeField] private float canvasFadeOutDuration = 1f;

    private Coroutine dialogueCoroutine;

    private void Start()
    {
        ShowDialogue(introMessages);
    }

    public void ShowIntroDialogue()
    {
        ShowDialogue(introMessages);
    }

    public void ShowAfterLevel1Dialogue()
    {
        ShowDialogue(afterLevel1Messages);
    }

    public void ShowAfterLevel2Dialogue()
    {
        ShowDialogue(afterLevel2Messages);
    }

    public void ShowAfterLevel3Dialogue()
    {
        ShowDialogue(afterLevel3Messages);
    }

    private void ShowDialogue(string[] messages)
    {
        if (messages == null || messages.Length == 0)
        {
            return;
        }

        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }

        gameObject.SetActive(true);

        canvasGroup.alpha = 1f;
        dialogueText.alpha = 0f;

        SetPlayerControl(false);

        dialogueCoroutine = StartCoroutine(PlayDialogueSequence(messages));
    }

    private IEnumerator PlayDialogueSequence(string[] messages)
    {
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

        yield return StartCoroutine(FadeCanvas(1f, 0f, canvasFadeOutDuration));

        SetPlayerControl(true);
        dialogueCoroutine = null;
        gameObject.SetActive(false);
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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
}