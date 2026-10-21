using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteraction : MonoBehaviour
{
    private InputAction interactAction;
     

    private void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
    }


    /*
    private void Update()
    {
        if (interactAction.IsPressed() && )
        {
            // Check if the player is in range of the door
            // If so, fade to black
        }
    }

    */


}
