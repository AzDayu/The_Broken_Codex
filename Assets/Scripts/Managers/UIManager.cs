using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private UIDocument _uiDocument;
    private Dictionary<string, UIBase> _uiViews = new Dictionary<string, UIBase>();

    void Awake()
    {
        Instance = this;
        _uiDocument = GetComponent<UIDocument>();

        var views = GetComponentsInChildren<UIBase>(true);
        Debug.Log($"[UIManager] 찾은 UI 뷰 개수: {views.Length}");

        foreach (var view in views)
        {
            view.Initialize(_uiDocument.rootVisualElement);
            _uiViews.Add(view.GetType().Name, view);
            Debug.Log($"[UIManager] {view.GetType().Name} 초기화 완료");
        }
    }

    void Start()
    {
        ShowUI<MainMenuUI>();
    }

    public void ShowUI<T>() where T : UIBase
    {
        string key = typeof(T).Name;
        if (_uiViews.TryGetValue(key, out var view))
        {
            view.Show();
        }
    }

    public void HideUI<T>() where T : UIBase
    {
        string key = typeof(T).Name;
        if (_uiViews.TryGetValue(key, out var view))
        {
            view.Hide();
        }
    }
}
