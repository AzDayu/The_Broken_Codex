using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager  Instance { get; private set; }

    private UIDocument mainUIDocument;

    [Header("UI Controllers")]
    public GameHUDUI gameHUD;
    public MainMenuUI mainMenuUI;
    // 추후 추가될 UI들
    // public PauseMenuUI pauseMenu;
    // public InventoryUI inventoryUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        mainUIDocument = GetComponent<UIDocument>();
        var root = mainUIDocument.rootVisualElement;

        if (mainMenuUI != null)
        {
            mainMenuUI.Initialize(root);
            mainMenuUI.Show();
        }


        if (gameHUD != null)
        {
            gameHUD.Initialize(root);
        }

        // if (pauseMenu != null) pauseMenu.Initialize(root);
        // 일시정지는 초기화만 하고 가만히 둡니다. (UIBase에 의해 자동으로 Hide 상태임)
    }

    public void TogglePauseMenu(bool isPaused)
    {
        // if (isPaused) pauseMenu.Show();
        // else pauseMenu.Hide();
    }
}
