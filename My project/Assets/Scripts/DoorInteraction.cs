using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DoorInteraction : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private Transform player;

    [Header("Dialogue")]
    [SerializeField] private DialogueSequence dialogueSequence;
  
    private InputAction interactAction;
    private bool canExit;
    private bool dialogueStarted;
    private LevelChanger levelChanger;
    private ScoreHandler scoreHandler;

    private void Start()
    {
        levelChanger = FindAnyObjectByType<LevelChanger>();
        interactAction = InputSystem.actions.FindAction("Interact");
        scoreHandler = FindAnyObjectByType<ScoreHandler>();

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canExit = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canExit = false;
        }
    }

    private void Update()
    {
        if (canExit)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                print("E-pressed");
                scoreHandler?.SaveCurrentLevelScore(); //William
                levelChanger.OnExitDoor();
            }
        }
    }
}
