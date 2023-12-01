

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

[Serializable]
public struct LanguageDictionaryTargetOptions
{
    public string Language;
    public string fullscreen;
    public string LanguageText;
    public string LoginText;
    public string MasterVolumeText;
}

public class Options : UIParent
{
    public static event Action<string> onLoginChange;
    public static event Action<string> onLanguageChange;
    
    public static string login;
    
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private Texture2D _backgroundOptions;
    [SerializeField] private List<string> languages;
    [SerializeField] private List<LanguageDictionaryTargetOptions> LanguageDictionaryTarget;
    [SerializeField] private AudioMixer masterAudioMixer;


    private VisualElement _root;
    private VisualElement backgroundOptions;
    private TextField loginOptions;
    private DropdownField dropdownLanguage;
    private Toggle toggleFullscreen;
    private LanguageDictionaryTargetOptions currentLanguage;
    private Slider masterVolume;
    
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
    
    public void closeOptions(ClickEvent ev)
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
        toggleFullscreen = _root.Q<Toggle>("Fullscreen");
        toggleFullscreen.RegisterValueChangedCallback((evt) => UpdateFullscreen(evt.newValue));
        toggleFullscreen.value = !PlayerPrefs.HasKey("Fullscreen") || PlayerPrefs.GetInt("Fullscreen") == 1;
        masterVolume = _root.Q<Slider>("MasterVolume");
        masterVolume.RegisterValueChangedCallback((evt) => UpdateVolume(evt.newValue));
        masterVolume.value = PlayerPrefs.HasKey("MasterVolume") ? PlayerPrefs.GetFloat("MasterVolume") : masterVolume.highValue;

        
        
        
        dropdownLanguage = _root.Q<DropdownField>("LanguageDropdown");
        dropdownLanguage.choices = languages;
        dropdownLanguage.RegisterValueChangedCallback(changeLanguage);
        dropdownLanguage.index = (PlayerPrefs.HasKey("language")) ? PlayerPrefs.GetInt("language") : 0;
    }

    public override void UpdateLanguage(string language)
    {
        currentLanguage = LanguageDictionaryTarget.Find(x => x.Language == language);
        if (currentLanguage.Language == null) return;
        toggleFullscreen.label = currentLanguage.fullscreen;
        dropdownLanguage.label = currentLanguage.LanguageText;
        loginOptions.label = currentLanguage.LoginText;
        masterVolume.label = currentLanguage.MasterVolumeText;
    }
    
    public void UpdateVolume(float volume)
    {
        masterAudioMixer.SetFloat("MasterVol", volume); 
    }
    
    public void UpdateFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
        PlayerPrefs.SetInt("Fullscreen", fullscreen ? 1 : 0);
    }

    private void changeLanguage(ChangeEvent<string> evt)
    {
        onLanguageChange?.Invoke(evt.newValue);
    }

    private void changeLogin(ChangeEvent<string> evt)
    {
        onLoginChange?.Invoke(evt.newValue);
        login = evt.newValue;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetString("login", loginOptions.value);
        PlayerPrefs.SetInt("language", dropdownLanguage.index);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume.value);
    }
}
