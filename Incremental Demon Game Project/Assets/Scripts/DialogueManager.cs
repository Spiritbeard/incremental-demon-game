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
    private DialogueExchangeScriptableObject.Facing previousFacing;
    private Queue<DialogueExchangeScriptableObject.Line> dialogueLines = new Queue<DialogueExchangeScriptableObject.Line>();
    public int currentLineIndex;
    private DialogueExchangeScriptableObject.Line currentLine;


    void Awake()
    {
        speakerAnimator = GetComponentInChildren<Animator>();
        autoTagSystem = GameObject.FindGameObjectWithTag("AutoTagSystem").GetComponent<AutoTagSystem>();
        previousFacing = DialogueExchangeScriptableObject.Facing.OnRight;
    }

    private void OnEnable()
    {
        //this will all move to wherever a new DialogueExchange is loaded in
        for (int i = 0; i < currentDialogueExchange.lines.Length; i++)
        {
            dialogueLines.Enqueue(currentDialogueExchange.lines[i]);
        }
        DisplayNextLine(currentDialogueExchange);
        //
        
        InputManager.OnEndPrimaryPress += ProgressDialogue;
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
                ResetAnimatorPosition();
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

        if (currentLine.speakerFacing == DialogueExchangeScriptableObject.Facing.OnLeft && previousFacing != DialogueExchangeScriptableObject.Facing.OnLeft)
        {
            previousFacing = DialogueExchangeScriptableObject.Facing.OnLeft;
            speakerAnimator.gameObject.transform.localPosition = new Vector2(speakerAnimator.gameObject.transform.localPosition.x - 400f, speakerAnimator.gameObject.transform.localPosition.y);
        }

        if (currentLine.speakerFacing == DialogueExchangeScriptableObject.Facing.OnRight && previousFacing != DialogueExchangeScriptableObject.Facing.OnRight)
        {
            previousFacing = DialogueExchangeScriptableObject.Facing.OnRight;
            speakerAnimator.gameObject.transform.localPosition = new Vector2(speakerAnimator.gameObject.transform.localPosition.x + 400f, speakerAnimator.gameObject.transform.localPosition.y);
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

    private void ResetAnimatorPosition()
    {
        if (previousFacing == DialogueExchangeScriptableObject.Facing.OnLeft)
        {
            speakerAnimator.gameObject.transform.localPosition = new Vector2(speakerAnimator.gameObject.transform.localPosition.x + 400, speakerAnimator.gameObject.transform.localPosition.y);
        }
        previousFacing = DialogueExchangeScriptableObject.Facing.OnRight;
    }

}
