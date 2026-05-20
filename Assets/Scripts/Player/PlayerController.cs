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

    private float currentMoveSpeed;
    private TileType currentTile = TileType.None;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    [SerializeField] private LayerMask groundLayer;
    private CapsuleCollider2D capsuleCollider;

    private UIManager uiManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        currentMoveSpeed = moveSpeed;
    }

    void Start()
    {
        uiManager = FindFirstObjectByType<UIManager>();
    }

    void Update()
    {
        CheckCurrentTile();
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            rb.linearVelocity = moveInput * currentMoveSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
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

    private void CheckCurrentTile()
    {
        if (MapManager.Instance == null) return;

        TileType detectedTile = MapManager.Instance.GetTileUnderPosition(transform.position);

        if (detectedTile != currentTile)
        {
            currentTile = detectedTile;
            ApplyTileEffect(currentTile);
        }
    }

    private void ApplyTileEffect(TileType tile)
    {
        Debug.Log($"[타일 확인] 현재 밟고 있는 타일: {tile}");

        switch (tile)
        {
            case TileType.Office:
            case TileType.Restored:
                currentMoveSpeed = moveSpeed;
                break;
            case TileType.Glitch:
                currentMoveSpeed = moveSpeed * 0.5f;
                Debug.LogWarning("글리치 타일 위를 걷는 중! 이동 속도 감소!");
                break;
            case TileType.None:
                Debug.LogError("맵 밖으로 벗어났습니다!");
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
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
    //}
}
