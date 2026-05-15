using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : UIBase
{
    private Button _startButton;
    private Button _quitButton;

    protected override void BindElements()
    {
        VisualElement startInstance = RootElement.Q<VisualElement>("main-btn-start");
        VisualElement quitInstance = RootElement.Q<VisualElement>("main-btn-quit");

        _startButton = startInstance.Q<Button>();
        _quitButton = quitInstance.Q<Button>();

        if (_startButton != null) _startButton.clicked += OnStartClicked;
        if (_quitButton != null) _quitButton.clicked += OnQuitClicked;
    }

    private void OnStartClicked()
    {
        Debug.Log("게임을 시작합니다!");
    }

    private void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
