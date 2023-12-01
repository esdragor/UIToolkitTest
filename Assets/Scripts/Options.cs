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
    public string SpeedDialogueText;
}

public class Options : UIParent
{
    public event Action<string> onLoginChange;
    public event Action<string> onLanguageChange;
    public event Action onCloseOptions;

    public static string login;
    public static float speedDialogue;
    public static string language;

    [SerializeField] private Texture2D _backgroundOptions;
    [SerializeField] private List<string> languages;
    [SerializeField] private List<LanguageDictionaryTargetOptions> LanguageDictionaryTarget;
    [SerializeField] private AudioMixer masterAudioMixer;


    private VisualElement backgroundOptions;
    private TextField loginOptions;
    private DropdownField dropdownLanguage;
    private Toggle toggleFullscreen;
    private LanguageDictionaryTargetOptions currentLanguage;
    private Slider masterVolume;
    private Slider SpeedDialogue;

    public override void Init()
    {
        base.Init();
        InitPanels();
        InitButtons();
        InitOthers();
        Hide();
    }

    public void OpenOptions()
    {
        _root.style.display = DisplayStyle.Flex;
    }

    private void closeOptions(ClickEvent ev)
    {
        onCloseOptions?.Invoke();
        Hide();
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
        changeLogin(loginOptions.value);
    }


    protected override void InitOthers()
    {
        toggleFullscreen = _root.Q<Toggle>("Fullscreen");
        toggleFullscreen.RegisterValueChangedCallback((evt) => UpdateFullscreen(evt.newValue));
        toggleFullscreen.value = !PlayerPrefs.HasKey("Fullscreen") || PlayerPrefs.GetInt("Fullscreen") == 1;
        masterVolume = _root.Q<Slider>("MasterVolume");
        masterVolume.RegisterValueChangedCallback((evt) => UpdateVolume(evt.newValue));
        masterVolume.value = PlayerPrefs.HasKey("MasterVolume")
            ? PlayerPrefs.GetFloat("MasterVolume")
            : masterVolume.highValue;
        SpeedDialogue = _root.Q<Slider>("SpeedDialogue");
        SpeedDialogue.RegisterValueChangedCallback((evt) => UpdateSpeedDialogue(evt.newValue));
        SpeedDialogue.value = PlayerPrefs.HasKey("SpeedDialogue")
            ? PlayerPrefs.GetFloat("SpeedDialogue")
            : SpeedDialogue.lowValue;
        
        dropdownLanguage = _root.Q<DropdownField>("Language");
        dropdownLanguage.choices = languages;
        dropdownLanguage.RegisterValueChangedCallback(changeLanguage);
        dropdownLanguage.index = (PlayerPrefs.HasKey("language")) ? PlayerPrefs.GetInt("language") : 0;
        
        backgroundOptions.style.backgroundImage = new StyleBackground(_backgroundOptions);
    }

    private void UpdateSpeedDialogue(float evtNewValue)
    {
        speedDialogue = evtNewValue;
    }

    public override void UpdateLanguage(string language)
    {
        currentLanguage = LanguageDictionaryTarget.Find(x => x.Language == language);
        if (currentLanguage.Language == null) return;
        toggleFullscreen.label = currentLanguage.fullscreen;
        dropdownLanguage.label = currentLanguage.LanguageText;
        loginOptions.label = currentLanguage.LoginText;
        masterVolume.label = currentLanguage.MasterVolumeText;
        SpeedDialogue.label = currentLanguage.SpeedDialogueText;
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
        if (!languages.Contains(evt.newValue)) return;
        onLanguageChange?.Invoke(evt.newValue);
    }

    private void changeLogin(ChangeEvent<string> evt)
    {
        if (evt.newValue == loginOptions.label) return;
        changeLogin(evt.newValue);
    }
    
    private void changeLogin(string loginOptionsValue)
    {
        onLoginChange?.Invoke(loginOptionsValue);
        login = loginOptionsValue;
    }


    private void OnDisable()
    {
        PlayerPrefs.SetString("login", loginOptions.value);
        PlayerPrefs.SetInt("language", dropdownLanguage.index);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume.value);
        PlayerPrefs.SetFloat("SpeedDialogue", SpeedDialogue.value);
    }
}