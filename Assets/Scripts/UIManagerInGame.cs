using System;
using UnityEngine;

public class UIManagerInGame : MonoBehaviour
{
    [SerializeField] private DialogueManager dialog;
    [SerializeField] private Options options;
    [SerializeField] private Pause pause;
    void Start()
    {
        pause.OnResume += pause.Hide;
        pause.OnResume += dialog.Show;
        
        pause.onOpenOptions += options.OpenOptions;
        pause.onOpenOptions += pause.Hide;
        
        pause.OnPauseOpen += pause.Show;
        
        options.onCloseOptions += pause.Show;
        
        dialog.OnEndOfDialogue += EndOfDialogue;
        
        options.onLanguageChange += dialog.UpdateLanguage;
        options.onLanguageChange += options.UpdateLanguage;
        options.onLanguageChange += pause.UpdateLanguage;
        
        dialog.Init();
        pause.Init();
        options.Init();
        dialog.LaunchDialogue();
    }

    private void OnDisable()
    {
        pause.OnResume -= pause.Hide;
        pause.OnResume -= dialog.Show;
        
        pause.onOpenOptions -= options.OpenOptions;
        pause.onOpenOptions -= pause.Hide;
        
        pause.OnPauseOpen -= pause.Show;
        
        options.onCloseOptions -= pause.Show;
        
        dialog.OnEndOfDialogue -= EndOfDialogue;
        
        options.onLanguageChange -= dialog.UpdateLanguage;
        options.onLanguageChange -= options.UpdateLanguage;
        options.onLanguageChange -= pause.UpdateLanguage;
    }

    private void EndOfDialogue()
    {
        pause.OnResume -= dialog.Show;
    }
    
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause.openPause();
        }
    }
}
