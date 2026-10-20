using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProximityTooltip : MonoBehaviour
{
    [Header("Proximity Settings")]
    public float triggerDistance = 3f; // How close the player needs to be
    public LayerMask interactableLayer; // Optional layer mask to filter objects
    [SerializeField] public Camera playerCamera;

    private Transform playerTransform;
    [SerializeField] private ToolTipManager toolTipManager; // Reference to the ToolTipManager

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        //CheckNearestObject();
        CheckMouseHoverObject();
    }

    void CheckMouseHoverObject()
    {
        if (Mouse.current == null) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = playerCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("Painting") || hitObject.CompareTag("Door") || hitObject.CompareTag("Plate"))
            {
                float distanceToPlayer = Vector3.Distance(playerTransform.position, hitObject.transform.position);

                if (distanceToPlayer <= triggerDistance)
                {
                    string message = GetMessageByTag(hitObject.tag);
                    toolTipManager.ShowTooltip(message);
                    return;
                }
            }
        }
        toolTipManager.HideTooltip();
    }


    string GetMessageByTag(string objectTag)
    {
        switch (objectTag)
        {
            case "Painting":
                return "[E] Burn ";
            case "Door":
                return "[E] Leave and report to the Boss ";
            case "Plate":
                return "[E] Read ";
            default:
                return " ";
        }
    }

    //Method 2


    /*void CheckNearestObject()
    {
        Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, triggerDistance);

        GameObject closestObject = null;
        float minDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Painting") || hitCollider.CompareTag("Door") || hitCollider.CompareTag("Plate"))
            {
                float dist = Vector3.Distance(playerTransform.position, hitCollider.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closestObject = hitCollider.gameObject;
                }
            }
        }

        if (closestObject != null)
        {
            string tag = closestObject.tag;
            string message = GetMessageByTag(tag);
            toolTipManager.ShowTooltip(message);
        }
        else
        {
            toolTipManager.HideTooltip();
        }
    }
    void CheckMouseHoverObject()
    {
        // 1. Create a ray from the camera passing through the Mouse Position on screen
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 2. Raycast into the 3D scene
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            GameObject hitObject = hit.collider.gameObject;

            // 3. Check if the player is close enough to the object (Proximity check)
            float distanceToPlayer = Vector3.Distance(transform.position, hitObject.transform.position);

            if (distanceToPlayer <= triggerDistance)
            {
                // 4. Check if it has one of your tags
                if (hitObject.CompareTag("Painting") || hitObject.CompareTag("Door") || hitObject.CompareTag("Plate"))
                {
                    string message = GetMessageByTag(hitObject.tag);
                    toolTipManager.ShowTooltip(message);
                    return;
                }
            }
        }

        // If we aren't hovering over a valid object within distance, hide tooltip
        toolTipManager.HideTooltip();
    }*/


}