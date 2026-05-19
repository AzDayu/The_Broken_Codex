using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] public float moveSpeed = 8f;
    [SerializeField] public float jumpForce = 15f;

    [Header("Status")]
    public bool canMove = true;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    [SerializeField] private LayerMask groundLayer;
    private CapsuleCollider2D capsuleCollider;

    private UIManager uiManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {
        uiManager = FindFirstObjectByType<UIManager>();
    }

    private void OnMove(InputValue value)
    {
        if (!canMove)
        {
            moveInput = Vector2.zero;
            return;
        }
        moveInput = value.Get<Vector2>();
    }

    private void OnJump()
    {
        if (moveInput != Vector2.zero)
        {
            rb.AddForce(moveInput * jumpForce);
        }
    }

    //private void OnOpen(InputValue value)
    //{
    //    if (value.isPressed)
    //    {
    //        if (UIManager.Instance != null)
    //        {
    //            UIManager.Instance.ToggleMenu();
    //        }
    //        else
    //        {
    //            Debug.LogError("UIManager Instance가 없습니다!");
    //        }
    //    }
    //}

    //private void OnAttack(InputValue value)
    //{
    //    if (!canMove) return;
    //
    //    if (value.isPressed)
    //    {
    //        animator.SetTrigger("Attack");
    //    }
    //}

    //public void ApplyPowerUp(float speedBoost, float jumpBoost, Color color, float duration)
    //{
    //    StartCoroutine(PowerUpRoutine(speedBoost, jumpBoost, color, duration));
    //}
    //
    //private IEnumerator PowerUpRoutine(float speedBoost, float jumpBoost, Color color, float duration)
    //{
    //    moveSpeed += speedBoost;
    //    jumpForce += jumpBoost;
    //    spriteRenderer.color = color;
    //
    //    Debug.Log("버프 시작!");
    //
    //    yield return new WaitForSeconds(duration);
    //
    //    moveSpeed -= speedBoost;
    //    jumpForce -= jumpBoost;
    //    spriteRenderer.color = originalColor;
    //
    //    Debug.Log("버프 종료, 원래대로 복구되었습니다.");
    //}

    private void FixedUpdate()
    {
        if (!canMove) return;

        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);
    }
}
