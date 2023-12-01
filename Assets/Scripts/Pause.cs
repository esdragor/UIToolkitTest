using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[Serializable]
public struct LanguageDictionaryTargetPause
{
    public string Language;
    public string Resume;
    public string Options;
    public string Quit;
}

public class Pause : UIParent
{
    public event Action OnResume;
    public event Action onOpenOptions;
    public event Action OnPauseOpen;
    
    [SerializeField] private List<LanguageDictionaryTargetPause> LanguageDictionaryTarget;

    
    private Button _resumeButton;
    private Button _optionsButton;
    private Button _quitButton;
    private LanguageDictionaryTargetPause currentLanguage;
    
    public override void Init()
    {
        base.Init();
        InitPanels();
        InitButtons();
        InitOthers();
        Hide();
    }

    protected override void InitPanels()
    {
        
    }

    protected override void InitButtons()
    {
        _resumeButton = _root.Q<Button>("Resume");
        _optionsButton = _root.Q<Button>("Options");
        _quitButton = _root.Q<Button>("Quit");
        
        _resumeButton.clicked += Resume;
        _optionsButton.clicked += () => onOpenOptions?.Invoke();
        _quitButton.clicked += () => SceneManager.LoadScene(0);
    }

    protected override void InitOthers()
    {
    }

    public override void UpdateLanguage(string language)
    {
        currentLanguage = LanguageDictionaryTarget.Find(x => x.Language == language);
        if (currentLanguage.Language == null) return;
        _resumeButton.text = currentLanguage.Resume;
        _optionsButton.text = currentLanguage.Options;
        _quitButton.text = currentLanguage.Quit;
    }

    private void Resume()
    {
        Time.timeScale = 1;
        OnResume?.Invoke();
        Hide();
    }

    public void openPause()
    {
        Time.timeScale = 0;
        Show();
        OnPauseOpen?.Invoke();
    }
}
