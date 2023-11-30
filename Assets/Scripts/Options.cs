using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;



public class Options : UIParent
{
    public static event Action<string> onLoginChange;
    public static event Action<string> onLanguageChange;
    
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private Texture2D _backgroundOptions;
    [SerializeField] private List<string> languages;

    private VisualElement _root;
    private VisualElement backgroundOptions;
    private TextField loginOptions;
    private DropdownField dropdownLanguage;
    
    public void Init()
    {
        InitPanels();
        InitButtons();
        InitOthers();
        _root.style.display = DisplayStyle.None;
        backgroundOptions.style.backgroundImage = new StyleBackground(_backgroundOptions);
    }

    public void OpenOptions()
    {
       _root.style.display = DisplayStyle.Flex;
    }
    
    void closeOptions(ClickEvent ev)
    {
        _root.style.display = DisplayStyle.None;
    }
    
    protected override void InitPanels()
    {
        _root = _uiDocument.rootVisualElement;
        backgroundOptions = _root.Q<VisualElement>("BackgroundOptions");
    }

    protected override void InitButtons()
    {
        var close = _root.Q<Button>("CloseOptionsBtn");
        loginOptions = _root.Q<TextField>("LoginOption");
       
        close.RegisterCallback<ClickEvent>(closeOptions);
        loginOptions.RegisterCallback<ChangeEvent<string>>(changeLogin);
        if (PlayerPrefs.HasKey("login"))
        {
            loginOptions.value = PlayerPrefs.GetString("login");
        }
        else
        {
            loginOptions.value = "random Guy";
        }
    }
    
    protected override void InitOthers()
    {
        dropdownLanguage = _root.Q<DropdownField>("LanguageDropdown");
        dropdownLanguage.choices = languages;
        dropdownLanguage.RegisterValueChangedCallback(changeLanguage);
        dropdownLanguage.index = (PlayerPrefs.HasKey("language")) ? PlayerPrefs.GetInt("language") : 0;
    }

    public override void UpdateLanguage(string language)
    {
        
    }

    private void changeLanguage(ChangeEvent<string> evt)
    {
        onLanguageChange?.Invoke(evt.newValue);
    }

    private void changeLogin(ChangeEvent<string> evt)
    {
        onLoginChange?.Invoke(evt.newValue);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetString("login", loginOptions.value);
        PlayerPrefs.SetInt("language", dropdownLanguage.index);
    }
}
