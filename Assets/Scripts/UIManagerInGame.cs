using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerInGame : MonoBehaviour
{
    [SerializeField] private DialogueManager dialog;
    [SerializeField] private Options options;
    void Start()
    {
        DialogueManager.OnEndOfDialogue += options.OpenOptions;
        options.Init();
        dialog.Init();
        options.closeOptions(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
