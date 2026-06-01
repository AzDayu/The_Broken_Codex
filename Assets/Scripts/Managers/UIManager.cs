using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager  Instance { get; private set; }

    private UIDocument mainUIDocument;

    [Header("UI Controllers")]
    public GameHUDUI gameHUD;
    public MainMenuUI mainMenuUI;

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

    }

}
