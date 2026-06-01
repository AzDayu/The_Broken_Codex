using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHUDUI : UIBase
{
    private Label playerInfoText;
    private VisualElement hpBarFill;
    private VisualElement staminaBarFill;
    private VisualElement buffListContainer;
    private VisualElement glitchProgressFill;
    private VisualElement shardProgressFill;

    private Label floorDepthText;
    private VisualElement radarIcon;
    private VisualElement systemLogContainer;
    private VisualElement tickFlashScreen;

    private Queue<Label> logQueue = new Queue<Label>();
    private int maxLogCount = 4;

    protected override void BindElements()
    {
        playerInfoText = RootElement.Q<Label>("Label_PlayerInfo");
        hpBarFill = RootElement.Q<VisualElement>("Gauge_HP_Fill");
        staminaBarFill = RootElement.Q<VisualElement>("Gauge_Stamina_Fill");
        buffListContainer = RootElement.Q<VisualElement>("Container_BuffList");
        glitchProgressFill = RootElement.Q<VisualElement>("Gauge_Glitch_Fill");
        shardProgressFill = RootElement.Q<VisualElement>("Gauge_Shard_Fill");

        floorDepthText = RootElement.Q<Label>("Label_FloorDepth");
        radarIcon = RootElement.Q<VisualElement>("Radar_Icon");
        systemLogContainer = RootElement.Q<VisualElement>("Container_SystemLog");
        tickFlashScreen = RootElement.Q<VisualElement>("Tick_Flash_Screen");
    }


    public void UpdatePlayerStats(float hp, float maxHp, float stamina, float maxStamina)
    {
        UpdateBarWidth(hpBarFill, hp, maxHp);
        UpdateBarWidth(staminaBarFill, stamina, maxStamina);
    }

    public void UpdateProgress(int glitchCount, int maxGlitch, int shardCount, int maxShard)
    {
        UpdateBarWidth(glitchProgressFill, glitchCount, maxGlitch);
        UpdateBarWidth(shardProgressFill, shardCount, maxShard);
    }

    private void UpdateBarWidth(VisualElement barFill, float current, float max)
    {
        if (barFill != null && max > 0)
        {
            float pct = Mathf.Clamp01(current / max) * 100f;
            barFill.style.width = new Length(pct, LengthUnit.Percent);
        }
    }

    public void AddSystemLog(string message)
    {
        if (systemLogContainer == null) return;

        Label newLog = new Label($"> {message}");
        newLog.style.color = new StyleColor(Color.green);
        newLog.style.fontSize = 14;

        systemLogContainer.Add(newLog);
        logQueue.Enqueue(newLog);

        if (logQueue.Count > maxLogCount)
        {
            Label oldLog = logQueue.Dequeue();
            systemLogContainer.Remove(oldLog);
        }
    }

    public void UpdateFloor(int floorNum)
    {
        if (floorDepthText != null)
            floorDepthText.text = $"SYSTEM DEPTH : {floorNum:D2}";
    }

    public void PlayTickEffect()
    {
        if (tickFlashScreen == null) return;

        tickFlashScreen.style.backgroundColor = new StyleColor(new Color(1, 1, 1, 0.1f));

        tickFlashScreen.schedule.Execute(() => {
            tickFlashScreen.style.backgroundColor = new StyleColor(Color.clear);
        }).StartingIn(100);
    }

    public void SetRadarWarning(bool isDangerNear)
    {
        if (radarIcon == null) return;
        radarIcon.style.backgroundColor = isDangerNear ? new StyleColor(Color.red) : new StyleColor(Color.gray);
    }
}
