using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] private DialogueSequence dialogueSequence;
    private LevelCounter levelCounter;
    private GameHandler gameHandler;
    [SerializeField] private SceneFade sceneFade;
    private ScoreHandler scoreHandler;

    void Start()
    {
        gameHandler = FindAnyObjectByType<GameHandler>();
        levelCounter = FindAnyObjectByType<LevelCounter>();
        scoreHandler = FindAnyObjectByType<ScoreHandler>();

        if (levelCounter.GetCurrantLevel() == 1 && levelCounter.GetCurrantLevel() != 6)
        {
            Debug.Log("First level reached starting handle first text");

            
            scoreHandler.ResetTotalScore();

        }

        if (levelCounter.GetCurrantLevel() == 6)
        {
            Debug.Log("Last level reached starting handle last text");
            StartCoroutine(HandleLastText());

        }
    }

    public void OnExitDoor()
    {
        Debug.Log("Exit door triggered");
        if (levelCounter.GetCurrantLevel() <6)
        {
            if (gameHandler.IsLevelComplete())
            {
                StartCoroutine(HandleSuccessfullTask());
            }
            else
            {
                StartCoroutine(HandleFailedTask());
            }
        }
        else
        {
           // StartCoroutine(HandleLastText());
        }
    }

    private IEnumerator HandleSuccessfullTask()
    {
        dialogueSequence.ShowSuccessfullMessage();

        yield return new WaitUntil(
            () => !dialogueSequence.IsDialoguePlaying);

        Debug.Log("Task completed successfully!");

        sceneFade.LoadScene("GameMainScene");
    }

    private IEnumerator HandleFailedTask()
    {
        dialogueSequence.ShowFaliedTaskMessage();

        yield return new WaitUntil(
            () => !dialogueSequence.IsDialoguePlaying);

        Debug.Log("Task failed!");

        Destroy(levelCounter.gameObject);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        sceneFade.LoadScene("MainMenu");
    }

    private IEnumerator HandleLastText()
    {
        dialogueSequence.ShowLastMessage();

        yield return new WaitUntil(
            () => !dialogueSequence.IsDialoguePlaying);

        Debug.Log("Mission Complete!");

        Destroy(levelCounter.gameObject);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("MainMenu");
    }


}
