using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUI : UIBase
{
    [Header("인벤토리 설정")]
    public int maxSlots = 20;

    private VisualElement slotGrid;
    private Label shardCountLabel;
    private Label playerNameLabel;
    private VisualElement playerPortrait;

    private List<VisualElement> itemSlots = new List<VisualElement>();

    public bool isShow = false;

    protected override void BindElements()
    {
        slotGrid = RootElement.Q<VisualElement>("Container_SlotGrid");
        shardCountLabel = RootElement.Q<Label>("Label_ShardCount");
        playerNameLabel = RootElement.Q<Label>("Label_PlayerName");
        playerPortrait = RootElement.Q<VisualElement>("Image_PlayerPortrait");

        InitializeSlots();

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged += RefreshUI;
        }

        Debug.Log("[UI] InventoryUI 바인딩 및 슬롯 생성 완료!");
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= RefreshUI;
        }
    }

    private void InitializeSlots()
    {
        if (slotGrid == null) return;

        slotGrid.Clear();
        itemSlots.Clear();

        for (int i = 0; i < maxSlots; i++)
        {
            VisualElement newSlot = new VisualElement();
            newSlot.AddToClassList("item-slot");

            Label amountText = new Label("");
            amountText.name = "Label_Amount";
            amountText.style.color = Color.white;

            newSlot.Add(amountText);
            slotGrid.Add(newSlot);
            itemSlots.Add(newSlot);
        }
    }

    public override void Show()
    {
        if (isShow == false)
        {
            base.Show();
            isShow = true;
        }
        else
        {
            base.Hide();
            isShow = false;
        }
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (InventoryManager.Instance == null) return;

        var currentInventory = InventoryManager.Instance.inventory;
        int index = 0;
        int shardCount = 0;

        foreach (var slot in itemSlots)
        {
            slot.style.backgroundImage = null;
            slot.Q<Label>("Label_Amount").text = "";
        }

        foreach (var kvp in currentInventory)
        {
            ItemData item = kvp.Key;
            int amount = kvp.Value;

            if (item.itemType == ItemType.GlitchShard)
            {
                shardCount += amount;
            }

            if (index >= itemSlots.Count) break;

            if (item.itemIcon != null)
            {
                itemSlots[index].style.backgroundImage = new StyleBackground(item.itemIcon);
            }

            if (amount > 1)
            {
                itemSlots[index].Q<Label>("Label_Amount").text = amount.ToString();
            }

            index++;
        }

        if (shardCountLabel != null)
        {
            shardCountLabel.text = $"Shards: {shardCount} / 5";
        }
    }
}