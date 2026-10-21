using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Movement
    [SerializeField] private float walkSpeed = 5.5f;
    [SerializeField] private float runningSpeed = 9.0f;

    private float currentMoveSpeed = 0.0f;
    private Vector3 moveDirection = Vector3.zero;

    // Footstep sounds
    [SerializeField] private AudioClip walkingSound;
    [SerializeField] private AudioClip sprintingSound;

    private AudioSource footstepAudioSource;

    //Sight
    [SerializeField] private float lookSensitivity = 0.2f;
    [SerializeField] private float lookAngleLimit = 90f;

    private float lookAngle = 0.0f;

    //refrences
    [SerializeField] private Camera m_Camera;
    private CharacterController characterController;

    public float normalFOV = 60f;
    public float zoomedFOV = 30f;
    public float zoomSpeed = 5f;

    //input (new system or old?)
    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction zoomAction;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        footstepAudioSource = GetComponent<AudioSource>();

        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        zoomAction = InputSystem.actions.FindAction("Zoom");

        //Detta har med cursor att göra... hur vill vi ha det
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentMoveSpeed = walkSpeed;
        m_Camera.fieldOfView = normalFOV;

        lookAngle = 0f;
        m_Camera.transform.localRotation = Quaternion.identity;

        footstepAudioSource.loop = true;
        footstepAudioSource.playOnAwake = false;
    }

    private void Update()
    {
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        Vector2 mouseDelta = new Vector2(Mouse.current.delta.x.ReadValue(), Mouse.current.delta.y.ReadValue());
        bool isSprinting = sprintAction.IsPressed();
        currentMoveSpeed = sprintAction.IsPressed() ? runningSpeed : walkSpeed;

        HandleMovement(moveVector);
        HandleLooking(mouseDelta);
        HandleCameraZoom();
        HandleFootsteps(moveVector, isSprinting);
    }

    private void HandleMovement(Vector2 moveVector)
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float oldY = moveDirection.y;

        //Vector2 newSpeed = new Vector2(moveVector.y * currentMoveSpeed, moveVector.y * currentMoveSpeed);

        float forwardMovement = moveVector.y * currentMoveSpeed;
        float sidewaysMovement = moveVector.x * currentMoveSpeed;

        moveDirection = (forward * forwardMovement) + (right * sidewaysMovement);
        moveDirection.y = oldY;

        characterController.Move(moveDirection * Time.deltaTime);

    }

    private void HandleFootsteps(Vector2 moveVector, bool isSprinting)
    {

        bool isMoving = moveVector.sqrMagnitude > 0.01f;

        if (!isMoving)
        {
            footstepAudioSource.Stop();
            return;
        }
        AudioClip activeSound = isSprinting ? sprintingSound : walkingSound;

        if (footstepAudioSource.clip != activeSound)
        {
            footstepAudioSource.clip = activeSound;
            footstepAudioSource.Play();
            Debug.Log("Playing footstep sound: " + activeSound.name);
        }
        else if (!footstepAudioSource.isPlaying)
        {
            footstepAudioSource.Play();
            Debug.Log("Playing footstep sound: " + activeSound.name);
        }
    }

    private void HandleLooking(Vector2 mouseDelta)
    {
        lookAngle += -mouseDelta.y * lookSensitivity;
        lookAngle = Mathf.Clamp(lookAngle, -lookAngleLimit, lookAngleLimit);

        m_Camera.transform.localRotation = Quaternion.Euler(lookAngle, 0, 0);
        transform.rotation *= Quaternion.Euler(0,mouseDelta.x * lookSensitivity, 0);
    }

    private void HandleCameraZoom()
    {
        float targetFOV = normalFOV;

        bool isZooming = (zoomAction != null && zoomAction.IsPressed());
        bool isNotZooming = (zoomAction != null && !zoomAction.IsPressed());

        if (isZooming)
        {
            targetFOV = zoomedFOV;
        }
        else if (isNotZooming)
        {
            targetFOV = normalFOV;
        }

        m_Camera.fieldOfView = Mathf.Lerp(m_Camera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }

    public void SetGameplayFOV()
    {
        if (m_Camera != null)
        {
            m_Camera.fieldOfView = normalFOV;
        }
    }
}
