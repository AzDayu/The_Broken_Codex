using UnityEngine;
using UnityEngine.UIElements;

public abstract class UIBase : MonoBehaviour
{
    protected VisualElement RootElement;
    [SerializeField] protected VisualTreeAsset LayoutAsset;

    public virtual void Initialize(VisualElement root)
    {
        if (LayoutAsset == null) return;

        RootElement = LayoutAsset.Instantiate();

        RootElement.style.flexGrow = 1;

        root.Add(RootElement);
        Hide();
        BindElements();
    }

    protected abstract void BindElements();

    public virtual void Show() => RootElement.style.display = DisplayStyle.Flex;
    public virtual void Hide() => RootElement.style.display = DisplayStyle.None;
}
