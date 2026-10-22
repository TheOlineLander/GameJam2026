using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] private DialogueSequence dialogueSequence;
    private LevelCounter levelCounter;
    private GameHandler gameHandler;

    void Start()
    {
        gameHandler = FindAnyObjectByType<GameHandler>();
        levelCounter = FindAnyObjectByType<LevelCounter>();
    }

    // Update is called once per frame
    void Update()
    {/*
        if (levelCounter.GetCurrantLevel() == 0)
        {
            dialogueSequence.ShowIntroDialogue();
        }
        else if (levelCounter.GetCurrantLevel() == 1)
        {
            dialogueSequence.ShowAfterLevel1Dialogue();
        }
        else if (levelCounter.GetCurrantLevel() == 2)
        {
            dialogueSequence.ShowAfterLevel2Dialogue();
        }*/
    }

    public void OnExitDoor()
    {
        Debug.Log("Exit door triggered");

        if (gameHandler.IsLevelComplete())
        {
            HandleSuccessfullTask();
        }
        else
        {
            HandleFailedTask();
        }
    }



    private void HandleSuccessfullTask()
    {
        //show message
        //Delay for 3 seconds /press space
        SceneManager.LoadScene("GameMainScene");
        Debug.Log("Task completed successfully!");
    }

    private void HandleFailedTask()
    {
        Destroy(levelCounter.gameObject);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
        // Handle the failed task here
        // For example, you can show a failure message or restart the level
        Debug.Log("Task failed!");
    }
}
