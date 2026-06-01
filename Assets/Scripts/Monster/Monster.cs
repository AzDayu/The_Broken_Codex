using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [Header("Identification")]
    public string monsterID;

    [Header("Data (Do Not Assign)")]
    public MonsterData myData;

    [Header("Drop Settings")]
    public GameObject shardPrefab;
    public int dropAmount = 3;

    protected Transform player;
    protected Rigidbody2D rb;
    protected float distanceToPlayer;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    public virtual void Initialize(MonsterData data)
    {
        myData = data;
        Debug.Log($"{gameObject.name} (ID: {monsterID}) 데이터 로드 완료! 속도: {myData.moveSpeed}");
    }

    protected virtual void Update()
    {
        if (player == null || myData == null) return;
        distanceToPlayer = Vector2.Distance(transform.position, player.position);
        ExecutePattern();
    }

    protected abstract void ExecutePattern();

    public virtual void Die()
    {
        if (rb != null) rb.linearVelocity = Vector2.zero;
        if (shardPrefab != null)
        {
            for (int i = 0; i < dropAmount; i++)
            {
                Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
                Instantiate(shardPrefab, (Vector2)transform.position + randomOffset, Quaternion.identity);
            }
        }
        Debug.Log($"{gameObject.name} 처치됨! 파편 드랍.");
        Destroy(gameObject);
    }
}
