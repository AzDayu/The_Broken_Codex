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

    public int hp = 100;

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        currentMoveSpeed = moveSpeed;
        UIManager.Instance.UpdateHP(hp);
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
            Vector2 checkPosition = (Vector2)transform.position + (moveInput.normalized * 0.3f);

            TileType nextTile = MapManager.Instance.GetTileUnderPosition(checkPosition);

            if (CheckIfWalkable(nextTile))
            {
                rb.linearVelocity = moveInput * currentMoveSpeed;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private bool CheckIfWalkable(TileType tileType)
    {
        if (tileType == TileType.Wall || tileType == TileType.None)
        {
            return false;
        }

        return true;
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
                break;
            case TileType.None:
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        // UI 업데이트 한 줄 호출!
        UIManager.Instance.UpdateHP(hp);
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
