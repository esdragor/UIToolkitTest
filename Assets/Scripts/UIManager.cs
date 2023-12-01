using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Menu _menu;
    [SerializeField] private Options _options;
    
    private void Start()
    {
        _menu.OnOpenOptions += _options.OpenOptions;
        _options.onLoginChange += _menu.UpdateLoginText;
        _options.onLanguageChange += _menu.UpdateLanguage;
        _options.onLanguageChange += _options.UpdateLanguage;
        
        _menu.Init();
        _options.Init();
    }
    
    private void OnDisable()
    {
        _menu.OnOpenOptions -= _options.OpenOptions;
        _options.onLoginChange -= _menu.UpdateLoginText;
        _options.onLanguageChange -= _menu.UpdateLanguage;
        _options.onLanguageChange -= _options.UpdateLanguage;
    }
}