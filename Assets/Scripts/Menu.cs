using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using UnityEngine.Video;

[Serializable]
public struct LanguageDictionaryTarget
{
    public string Language;
    public string Title;
    public string Login;
}

public class Menu : UIParent
{
    public static event Action OnOpenOptions;

    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private RenderTexture _renderTextureLoader;
    [SerializeField] private Texture2D _textureBackground;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private float loadSpeed = 0.005f;
    [SerializeField] private List<LanguageDictionaryTarget> LanguageDictionaryTarget;

    private VisualElement _root;
    private VisualElement backgroundMenu;
    private VisualElement backgroundBattle;
    private VisualElement parentMenuBtn;
    private ProgressBar ProgressBar;
    private Label LoginLabel;
    private string loginTxt;
    private Label _Title;
    private LanguageDictionaryTarget currentLanguage;

    public void Init()
    {
        currentLanguage = LanguageDictionaryTarget.Find(x => x.Language == "English");
        InitPanels();
        InitButtons();
        InitOthers();
    }

    protected override void InitOthers()
    {
        ProgressBar = _root.Q<ProgressBar>("NyahBar");
        LoginLabel = _root.Q<Label>("LoginText");
        _Title = _root.Q<Label>("Title");
    }

    public override void UpdateLanguage(string language)
    {
        // UpdateLoginText(language);
        // //UpdateTitle(language);
        // UpdateText("Login", LoginLabel, language);
        // UpdateText("Title", _Title, language);
        currentLanguage = LanguageDictionaryTarget.Find(x => x.Language == language);
        _Title.text = currentLanguage.Title;
        LoginLabel.text = currentLanguage.Login + loginTxt;
    }

    // private void UpdateText(LanguageMode language)
    // {
    //     LanguageDictionaryTarget target = LanguageDictionaryTarget.Find(x => x.Language == language);
    //     _Title.text = target.Title;
    //     LoginLabel.text = target.Login  + loginTxt;
    // }

    public void UpdateLoginText(string str)
    {
        if (str != null)
            loginTxt = str;
        LoginLabel.text = currentLanguage.Login + loginTxt;
    }

    protected override void InitPanels()
    {
        _root = _uiDocument.rootVisualElement;
        backgroundMenu = _root.Q<VisualElement>("BackgroundMenu");
        backgroundBattle = _root.Q<VisualElement>("BackgroundBattle");
        backgroundMenu.style.backgroundImage = new StyleBackground(_textureBackground);

        if (ProgressBar != null)
            ProgressBar.visible = false;
        backgroundMenu.style.display = DisplayStyle.Flex;

        backgroundBattle.style.display = DisplayStyle.None;
    }

    protected override void InitButtons()
    {
        parentMenuBtn = _root.Q<VisualElement>("BtnMenuParent");
        var button = _root.Q<Button>("LaunchBattleBtn");
        var quitButton = _root.Q<Button>("QuitBtn");
        var optionsButton = _root.Q<Button>("OptionsBtn");
        optionsButton.RegisterCallback<ClickEvent>(OpenOptions);
        quitButton.RegisterCallback<ClickEvent>(QuitGame);
        button.RegisterCallback<ClickEvent, string>(LaunchGame, "nya ?");
    }

    void LaunchGame(ClickEvent ev, string str)
    {
        StartCoroutine(LaunchBattle(str));
    }

    void QuitGame(ClickEvent ev)
    {
        Application.Quit();
    }

    void OpenOptions(ClickEvent ev)
    {
        OnOpenOptions?.Invoke();
    }

    IEnumerator LaunchBattle(string str)
    {
        ProgressBar.visible = true;
        ProgressBar.value = 0;
        parentMenuBtn.style.display = DisplayStyle.None;
        videoPlayer.Play();
        backgroundMenu.style.backgroundImage = new StyleBackground(Background.FromRenderTexture(_renderTextureLoader));
        string title = ProgressBar.title;
        ProgressBar.title = str + title;
        while (ProgressBar.value < ProgressBar.highValue)
        {
            ProgressBar.value += loadSpeed;
            yield return new WaitForSeconds(0.01f);
        }

        ProgressBar.visible = false;
        videoPlayer.Stop();
        backgroundMenu.style.backgroundImage = new StyleBackground(_textureBackground);
        parentMenuBtn.style.display = DisplayStyle.Flex;
        backgroundMenu.style.display = DisplayStyle.None;

        backgroundBattle.style.display = DisplayStyle.Flex;
    }
}