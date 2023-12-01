using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using UnityEngine.Video;

[Serializable]
public struct LanguageDictionaryTargetMenu
{
    public string Language;
    public string Title;
    public string Login;
    public string LaunchBtn;
    public string Options;
    public string Credits;
    public string Quit;
}

public class Menu : UIParent
{
    public static event Action OnOpenOptions;

    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private RenderTexture _renderTextureLoader;
    [SerializeField] private Texture2D _textureBackground;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private float loadSpeed = 0.005f;
    [SerializeField] private List<LanguageDictionaryTargetMenu> LanguageDictionaryTarget;

    private VisualElement _root;
    private VisualElement backgroundMenu;
    private VisualElement backgroundBattle;
    private VisualElement parentMenuBtn;
    private ProgressBar ProgressBar;
    private Label LoginLabel;
    private string loginTxt;
    private Label _Title;
    private Button _LaunchBattleBtn;
    private Button _OptionsBtn;
    private Button _CreditsBtn;
    private Button _QuitBtn;
    private LanguageDictionaryTargetMenu currentLanguage;

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
        currentLanguage = LanguageDictionaryTarget.Find(x => x.Language == language);
        if (currentLanguage.Language == null) return;
        _Title.text = currentLanguage.Title;
        LoginLabel.text = currentLanguage.Login + loginTxt;
        _LaunchBattleBtn.text = currentLanguage.LaunchBtn;
        _OptionsBtn.text = currentLanguage.Options;
        _CreditsBtn.text = currentLanguage.Credits;
        _QuitBtn.text = currentLanguage.Quit;
    }

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
        _OptionsBtn = _root.Q<Button>("OptionsBtn");
        _LaunchBattleBtn = _root.Q<Button>("LaunchBattleBtn");
        _CreditsBtn = _root.Q<Button>("CreditsBtn");
        _QuitBtn = _root.Q<Button>("QuitBtn");
        
        _OptionsBtn.RegisterCallback<ClickEvent>(OpenOptions);
        _QuitBtn.RegisterCallback<ClickEvent>(QuitGame);
        _LaunchBattleBtn.RegisterCallback<ClickEvent, string>(LaunchGame, "nya ?");
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