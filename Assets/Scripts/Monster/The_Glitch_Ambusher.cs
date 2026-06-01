using UnityEngine;

public class The_Glitch_Ambusher : Monster
{
    [Header("Ambusher Settings")]
    public float attackRange = 1.5f;
    public LayerMask obstacleLayer;

    private float cooldownTimer = 0f;

    private Vector2 patrolDirection;
    private float patrolTimer = 0f;
    private float changePatrolDirTime = 2f;

    protected override void ExecutePattern()
    {
        cooldownTimer += Time.deltaTime;

        if (distanceToPlayer <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else if (distanceToPlayer <= myData.detectRange && cooldownTimer >= myData.specialCooldown)
        {
            TryTeleportBehindPlayer();
            cooldownTimer = 0f;
        }
        else if (distanceToPlayer <= myData.detectRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * myData.moveSpeed;
        }
        else
        {
            PatrolAround();
        }
    }

    private void TryTeleportBehindPlayer()
    {
        Vector2 dirToPlayer = (player.position - transform.position).normalized;
        Vector2 targetPos = (Vector2)player.position + (dirToPlayer * 1.5f);

        bool isBlocked = Physics2D.OverlapCircle(targetPos, 0.4f, obstacleLayer);

        if (!isBlocked)
        {
            rb.linearVelocity = Vector2.zero;
            transform.position = targetPos;
            Debug.Log("글리치 잠복자: 기습 순간이동 성공!");
        }
        else
        {
            Debug.Log("글리치 잠복자: 뒷공간이 막혀 걸어서 다가갑니다.");
        }
    }

    private void PatrolAround()
    {
        patrolTimer += Time.deltaTime;

        if (patrolTimer >= changePatrolDirTime)
        {
            patrolDirection = Random.insideUnitCircle.normalized;
            patrolTimer = 0f;
        }

        rb.linearVelocity = patrolDirection * (myData.moveSpeed * 0.5f);
    }
}