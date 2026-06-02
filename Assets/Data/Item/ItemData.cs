using UnityEngine;

public enum ItemType
{
    GlitchShard,  
    Consumable,
    Weapon,
    Armor 
}

[CreateAssetMenu(fileName = "New Item", menuName = "The Broken Codex/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemID;
    public string itemName;
    public Sprite itemIcon;
    public ItemType itemType;
    [TextArea] public string description;
    public int value;
    public bool isStackable;
}
