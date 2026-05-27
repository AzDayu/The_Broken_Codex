using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : UIBase
{
    private Button startBtn;
    private Button quitBtn;

    protected override void BindElements()
    {
        startBtn = RootElement.Q<Button>("StartButton");
        quitBtn = RootElement.Q<Button>("QuitButton");

        if (startBtn != null)
        {
            startBtn.clicked += OnStartClicked;
            Debug.Log("StartButton 이벤트 연결 성공!");
        }
        else
        {
            Debug.LogError("StartButton을 UXML에서 찾을 수 없습니다. 이름을 확인해 주세요.");
        }

        if (quitBtn != null)
        {
            quitBtn.clicked += OnQuitClicked;
            Debug.Log("QuitButton 이벤트 연결 성공!");
        }
        else
        {
            Debug.LogError("QuitButton을 UXML에서 찾을 수 없습니다. 이름을 확인해 주세요.");
        }
    }

    private void OnStartClicked()
    {
        Hide();
        if (UIManager.Instance.gameHUD != null)
        {
            UIManager.Instance.gameHUD.Show();
        }
    }

    private void OnQuitClicked()
    {
        Debug.Log("게임을 종료합니다.");
        Application.Quit();
    }
}
