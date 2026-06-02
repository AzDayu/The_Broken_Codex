using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public System.Action OnInventoryChanged;

    public Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(ItemData item, int amount = 1)
    {
        if (item.isStackable && inventory.ContainsKey(item))
        {
            inventory[item] += amount;
        }
        else
        {
            inventory.Add(item, amount);
        }

        Debug.Log($"[인벤토리] {item.itemName} 획득! 현재 수량: {inventory[item]}");

        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(ItemData item, int amount = 1)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] -= amount;
            if (inventory[item] <= 0)
            {
                inventory.Remove(item);
            }
            Debug.Log($"[인벤토리] {item.itemName} 사용/소모됨.");

            OnInventoryChanged?.Invoke();
        }
    }
}