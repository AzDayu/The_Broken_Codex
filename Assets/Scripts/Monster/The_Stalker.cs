using UnityEngine;

public class The_Stalker : Monster
{
    [Header("Stalker Settings")]
    public float speedUpRange = 5f;
    public float attackRange = 1.5f;
    public float speedMultiplier = 1.5f;

    protected override void ExecutePattern()
    {
        if (distanceToPlayer <= attackRange)
        {
            rb.linearVelocity = Vector2.zero; 
        }
        else if (distanceToPlayer <= speedUpRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * (myData.moveSpeed * speedMultiplier);
        }
        else
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * myData.moveSpeed;
        }
    }
}