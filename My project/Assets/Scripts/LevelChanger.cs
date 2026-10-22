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
    {
  
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
        dialogueSequence.ShowSuccessfullMessage();
        //show message
        //Delay for 3 seconds /press space
        SceneManager.LoadScene("GameMainScene");
        Debug.Log("Task completed successfully!");
    }

    private void HandleFailedTask()
    {
        dialogueSequence.ShowFaliedTaskMessage();
        Destroy(levelCounter.gameObject);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Task failed!");
    }
}
