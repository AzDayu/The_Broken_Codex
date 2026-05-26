using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private Label hpText;
    private Label shardText;
    private ProgressBar glitchGauge;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Debug.Log("UIManager 실행");
    }

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        hpText = root.Q<Label>("HPText");
        shardText = root.Q<Label>("ShardText");
        glitchGauge = root.Q<ProgressBar>("GlitchGauge");
    }


   public void UpdateHP(int currentHP)
   {
       if (hpText != null)
           hpText.text = $"HP: {currentHP}";
   }

    public void UpdateShards(int currentShards, int maxShards)
    {
        if (shardText != null)
            shardText.text = $"파편: {currentShards} / {maxShards}";
    }

    public void UpdateGlitchGauge(float currentValue, float maxValue)
    {
        if (glitchGauge != null)
        {
            glitchGauge.value = currentValue;
            glitchGauge.highValue = maxValue;
            glitchGauge.title = $"Glitch: {(currentValue / maxValue) * 100:0}%";
            Debug.Log("glitchGauge 찾음");
        }
    }
}
