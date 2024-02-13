using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Localization;

public class DialogueManager : MonoBehaviour
{
    public DialogueExchangeScriptableObject currentDialogueExchange;
    public TMPro.TextMeshProUGUI speakerNameText;
    public TMPro.TextMeshProUGUI dialogueText;
    public bool isDialogueActive;
    private bool isExchangeOver;
    private AutoTagSystem autoTagSystem;
    private Animator speakerAnimator;
    private AnimationClip previousAnimation;
    private Queue<DialogueExchangeScriptableObject.Line> dialogueLines = new Queue<DialogueExchangeScriptableObject.Line>();
    public int currentLineIndex;
    private DialogueExchangeScriptableObject.Line currentLine;


    void Awake()
    {
        speakerAnimator = GetComponentInChildren<Animator>();
        autoTagSystem = GameObject.FindGameObjectWithTag("AutoTagSystem").GetComponent<AutoTagSystem>();
    }

    private void OnEnable()
    {
        for (int i = 0; i < currentDialogueExchange.lines.Length; i++)
        {
            dialogueLines.Enqueue(currentDialogueExchange.lines[i]);
        }
        InputManager.OnEndPrimaryPress += ProgressDialogue;
        DisplayNextLine(currentDialogueExchange);
    }

    private void OnDisable()
    {
        InputManager.OnEndPrimaryPress -= ProgressDialogue;
    }



    public void ProgressDialogue(Vector2 pos, float time)
    {
        StartDialogue(currentDialogueExchange);
    }

    private void StartDialogue(DialogueExchangeScriptableObject currentDialogueExchange)
    {       
        isDialogueActive = true;
        DisplayNextLine(currentDialogueExchange);
        gameObject.SetActive(true);
    }

    public void DisplayNextLine(DialogueExchangeScriptableObject currentDialogueExchange)
    {
        if (dialogueLines.Count == 0)
        {
            if (!isExchangeOver)
            {
                
            }
            else
            {
                EndConversation();
                return;
            }
        }

        currentLine = dialogueLines.Dequeue();
        speakerNameText.text = currentLine.speakerName;
        dialogueText.text = autoTagSystem.SetAutoTags(currentLine.lineText.GetLocalizedString());
        if (currentLine.speakerAnimation != previousAnimation)
        {
            speakerAnimator.SetTrigger(currentLine.speakerAnimation.name.ToString()+"Trigger");
            previousAnimation = currentLine.speakerAnimation;
        }

        if (dialogueLines.Count == 0)
        {
            isExchangeOver = true;
        }
    }

    private void EndConversation()
    {
        dialogueLines.Clear();
        isExchangeOver = true;
        isDialogueActive = false;
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

}
