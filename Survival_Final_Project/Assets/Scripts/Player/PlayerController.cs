using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumpForce;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;

    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true;

    private Rigidbody rig;
    private bool jumpRequest;

    public static PlayerController instance;

    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;
    private SurvivalWinCondition survivalManager;

    void Awake()
    {
        rig = GetComponent<Rigidbody>();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple PlayerController instances detected. Destroying extra instance.");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        Move();
        if (jumpRequest)
        {
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequest = false;
        }
    }

    void LateUpdate()
    {
        if (canLook)
            CameraLook();
    }

    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rig.velocity.y;
        rig.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            jumpRequest = true;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");
            PlayerDie();
        }
        else
        {
            Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");
        }
    }

    void PlayerDie()
    {
        Debug.Log("Player is dead");
        if (survivalManager != null)
        {
            survivalManager.LoseGame();
            Debug.Log("LoseGame() called successfully.");
        }
        else
        {
            Debug.LogError("SurvivalWinCondition not set or not found.");
        }
    }

    

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + Vector3.up * 0.01f, Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + Vector3.up * 0.01f, Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + Vector3.up * 0.01f, Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + Vector3.up * 0.01f, Vector3.down)
        };

        foreach (Ray ray in rays)
        {
            if (Physics.Raycast(ray.origin, ray.direction, 0.2f, groundLayerMask))
                return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + (transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.forward * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (transform.right * 0.2f), Vector3.down);
        Gizmos.DrawRay(transform.position + (-transform.right * 0.2f), Vector3.down);
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
