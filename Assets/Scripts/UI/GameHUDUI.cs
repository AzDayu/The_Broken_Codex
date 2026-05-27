using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHUDUI : UIBase
{
    // [코어 요소 6가지]
    private Label playerInfoText;        // 검정: 이름/레벨
    private VisualElement hpBarFill;       // 빨강: HP
    private VisualElement staminaBarFill;  // 파랑: 스테미나
    private VisualElement buffListContainer; // 노랑: 버프 슬롯
    private VisualElement glitchProgressFill;// 보라: 글리치 위험도
    private VisualElement shardProgressFill; // 초록: 파편 진행도

    // [추가 심화 요소 4가지]
    private Label floorDepthText;        // 중앙: 층수
    private VisualElement radarIcon;       // 우측 상단: 레이더
    private VisualElement systemLogContainer;// 좌측 하단: 로그 창
    private VisualElement tickFlashScreen; // 화면 테두리: 틱(턴) 인디케이터

    // 시스템 로그 관리를 위한 리스트
    private Queue<Label> logQueue = new Queue<Label>();
    private int maxLogCount = 4; // 화면에 띄울 최대 로그 개수

    protected override void BindElements()
    {
        // 1. 코어 요소 바인딩
        playerInfoText = RootElement.Q<Label>("Label_PlayerInfo");
        hpBarFill = RootElement.Q<VisualElement>("Gauge_HP_Fill");
        staminaBarFill = RootElement.Q<VisualElement>("Gauge_Stamina_Fill");
        buffListContainer = RootElement.Q<VisualElement>("Container_BuffList");
        glitchProgressFill = RootElement.Q<VisualElement>("Gauge_Glitch_Fill");
        shardProgressFill = RootElement.Q<VisualElement>("Gauge_Shard_Fill");

        // 2. 추가 요소 바인딩
        floorDepthText = RootElement.Q<Label>("Label_FloorDepth");
        radarIcon = RootElement.Q<VisualElement>("Radar_Icon");
        systemLogContainer = RootElement.Q<VisualElement>("Container_SystemLog");
        tickFlashScreen = RootElement.Q<VisualElement>("Tick_Flash_Screen");
    }

    #region 코어 기능 (스탯, 진행도, 버프)

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
    #endregion

    #region 추가 심화 기능 (로그, 층수, 틱, 레이더)

    // 1. [좌측 하단] 시스템 로그 텍스트 띄우기
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
    #endregion
}
