using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteraction : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private Transform player;

    [Header("Dialogue")]
    [SerializeField] private DialogueSequence dialogueSequence;
  
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

        if (dialogueStarted)
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
            Debug.Log("Player interacted with the door.");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
