using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Menu _menu;
    [SerializeField] private Options _options;
    
    private void Start()
    {
        Menu.OnOpenOptions += _options.OpenOptions;
        Options.onLoginChange += _menu.UpdateLoginText;
        Options.onLanguageChange += _menu.UpdateLanguage;
        Options.onLanguageChange += _options.UpdateLanguage;
        
        _menu.Init();
        _options.Init();
    }
}