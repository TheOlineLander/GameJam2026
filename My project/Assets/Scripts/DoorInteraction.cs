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
    private bool hasLeftLevel;
    private bool dialogueStarted;
    private LevelChanger levelChanger;
    private ScoreHandler scoreHandler;

    private AudioSource audio;

    private void Start()
    {
        levelChanger = FindAnyObjectByType<LevelChanger>();
        interactAction = InputSystem.actions.FindAction("Interact");
        scoreHandler = FindAnyObjectByType<ScoreHandler>();
        audio = GetComponent<AudioSource>();

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    public bool SetCanExit(bool value)
    {
        canExit = value;
        return canExit;
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
        if (canExit && !hasLeftLevel)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                hasLeftLevel = true;
                audio.PlayOneShot(audio.clip);
                //print("E-pressed");
                scoreHandler?.SaveCurrentLevelScore(); //William
                levelChanger.OnExitDoor();
            }
        }
    }
}
