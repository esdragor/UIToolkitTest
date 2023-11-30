using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIParent : MonoBehaviour
{
    protected abstract void InitPanels();
    protected abstract void InitButtons();
    protected abstract void InitOthers();
    
    public abstract void UpdateLanguage(string language);
}
