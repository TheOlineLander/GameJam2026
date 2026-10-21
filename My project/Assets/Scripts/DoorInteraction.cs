using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteraction : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private Transform player;

    [Header("Dialogue")]
    [SerializeField] private DialogueSequence dialogueSequence;
    [SerializeField] private bool canInteractOnlyOnce = true;

    private InputAction interactAction;
    private bool dialogueStarted;

    private void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    private void Update()
    {
        if (player == null || interactAction == null)
        {
            return;
        }

        if (canInteractOnlyOnce && dialogueStarted)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(
            transform.position,
            player.position);

        bool playerIsCloseEnough = distanceToPlayer <= interactionRange;
        bool interactPressed = interactAction.WasPressedThisFrame();

        if (playerIsCloseEnough && interactPressed)
        {
            StartLevel1Dialogue();
        }
    }

    private void StartLevel1Dialogue()
    {
        if (dialogueSequence == null)
        {
            Debug.LogWarning(
                "No DialogueSequence has been assigned to this door.",
                this);

            return;
        }

        dialogueStarted = true;
        dialogueSequence.ShowAfterLevel1Dialogue();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
