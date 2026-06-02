using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;
    public int amount = 1;

    private void Start()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr != null && itemData != null && itemData.itemIcon != null)
        {
            sr.sprite = itemData.itemIcon;
            col.size = sr.sprite.bounds.size;
        }
        else
        {
            col.size = new Vector2(1f, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager.Instance.AddItem(itemData, amount);

            Destroy(gameObject);
        }
    }
}
