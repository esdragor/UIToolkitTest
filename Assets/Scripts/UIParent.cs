using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class UIParent : MonoBehaviour
{
    [SerializeField] protected UIDocument _uiDocument;
    
    protected VisualElement _root;

    public virtual void Init()
    {
        _root = _uiDocument.rootVisualElement;
    }
    protected abstract void InitPanels();
    protected abstract void InitButtons();
    protected abstract void InitOthers();
    
    public abstract void UpdateLanguage(string language);

    
    
    public void Show()
    {
        _root.style.display = DisplayStyle.Flex;
    }
    
    public void Hide()
    {
        _root.style.display = DisplayStyle.None;
    }
}
