using UnityEngine;
using System.Collections;

public class The_Weeping_Observer : Monster
{
    public float attackCooldown = 3f;
    private float attackTimer = 0f;

    protected override void Awake()
    {
        base.Awake();
        if (rb != null) rb.bodyType = RigidbodyType2D.Static;
    }

    protected override void ExecutePattern()
    {
        if (distanceToPlayer > myData.detectRange) return;

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            AoEAttack();
            attackTimer = 0f;
        }
    }

    private void AoEAttack()
    {
        Debug.Log("눈물 흘리는 감시자: 플레이어 주변 광역 공격 발동!");
    }

    public void Die()
    {
        Debug.Log("보스 처치! 대량의 파편 생성");
        StartCoroutine(EndingRoutine());
    }

    private IEnumerator EndingRoutine()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("엔딩 씬으로 전환");
    }
}