using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using System;

public class DialogueManager : MonoBehaviour
{
    
    public Canvas dialogueCanvas;

    [SerializeField]
    public DialogueExchangeScriptableObject currentDialogueExchange;
    public TMPro.TextMeshProUGUI speakerNameText;
    public TMPro.TextMeshProUGUI dialogueText;
    public bool isDialogueActive;
    private bool isExchangeOver;
    private AutoTagSystem autoTagSystem;
    private Animator speakerAnimator;
    private AnimationClip previousAnimation;
    public AnimationClip noAnimation;
    private DialogueExchangeScriptableObject.Facing previousFacing;
    private float animationOffset;
    private Queue<DialogueExchangeScriptableObject.Line> dialogueLines = new Queue<DialogueExchangeScriptableObject.Line>();
    public int currentLineIndex;
    private DialogueExchangeScriptableObject.Line currentLine;

    public static event Action OnStartDialogue;
    public static event Action OnEndDialogue;
       
    //Looping functionality is for playtesting only. It will be removed in the final build.
    public bool isLooping;
    public bool wasCloseButtonPressed;
    //

    void Awake()
    {
        isDialogueActive = false;
        dialogueCanvas = GetComponent<Canvas>();
        dialogueCanvas.enabled = false;
        speakerAnimator = GetComponentInChildren<Animator>();
        speakerAnimator.enabled = false;
        isLooping = false; //Looping functionality is for playtesting only. It will be removed in the final build.
        autoTagSystem = GameObject.FindGameObjectWithTag("AutoTagSystem").GetComponent<AutoTagSystem>();
        previousFacing = DialogueExchangeScriptableObject.Facing.OnRight;
    }

    private void OnEnable()
    {
        InputManager.OnEndPrimaryPress += ProgressDialogue;
        ScreenResolutionManager.OnResolutionChange += UpdateAnimatorOffset;
        UpdateAnimatorOffset(Screen.width, Screen.height);
        UpdateAnimatorPosition(previousFacing);
    }

    private void OnDisable()
    {
        InputManager.OnEndPrimaryPress -= ProgressDialogue;
        ScreenResolutionManager.OnResolutionChange -= UpdateAnimatorOffset;
    }

    public void ProgressDialogue(Vector2 pos, float time)
    {
        if (isDialogueActive == false)
        {
            return;
        }
        if (isDialogueActive == true)
        {
            DisplayNextLine(currentDialogueExchange);
        }
    }

    public void StartDialogue(DialogueExchangeScriptableObject currentDialogueExchange)
    {       
        isDialogueActive = true;
        OnStartDialogue?.Invoke();
        isExchangeOver = false;
        dialogueCanvas.enabled = true;        
        previousAnimation = noAnimation;
        speakerAnimator.enabled = true;
        speakerAnimator.Rebind();
        speakerAnimator.Update(0f);
        dialogueLines.Clear();
        for (int i = 0; i < currentDialogueExchange.lines.Length; i++)
        {
            dialogueLines.Enqueue(currentDialogueExchange.lines[i]);
        }
        DisplayNextLine(currentDialogueExchange);
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
                EndDialogue();
                return;
            }
        }

        currentLine = dialogueLines.Dequeue();

        speakerNameText.text = currentLine.speakerName;

        dialogueText.text = autoTagSystem.SetAutoTags(currentLine.lineText.GetLocalizedString());
        
        if (currentLine.speakerAnimation != previousAnimation || previousAnimation == noAnimation)
        {
            UpdateAnimation(currentLine.speakerAnimation);
            Debug.Log("Animation changed to " + currentLine.speakerAnimation.name);
        }

        if (currentLine.speakerFacing != previousFacing)
        {  
            UpdateAnimatorPosition(currentLine.speakerFacing);
        }

        if (dialogueLines.Count == 0)
        {
            isExchangeOver = true;
        }
    }
    
    private void UpdateAnimation(AnimationClip newAnimation)
    {
        speakerAnimator.SetTrigger(newAnimation.name.ToString()+"Trigger");
        previousAnimation = newAnimation;
    }

    private void UpdateAnimatorPosition(DialogueExchangeScriptableObject.Facing facing)
    {
        switch (facing)
        {
            case DialogueExchangeScriptableObject.Facing.OnLeft:
                speakerAnimator.gameObject.transform.localPosition = new Vector2(speakerAnimator.gameObject.transform.position.x - animationOffset, speakerAnimator.gameObject.transform.localPosition.y);
                previousFacing = DialogueExchangeScriptableObject.Facing.OnLeft;
                break;
            case DialogueExchangeScriptableObject.Facing.InCenter:
                speakerAnimator.gameObject.transform.localPosition = new Vector2(speakerAnimator.gameObject.transform.position.x, speakerAnimator.gameObject.transform.localPosition.y);
                previousFacing = DialogueExchangeScriptableObject.Facing.InCenter;
                break;
            case DialogueExchangeScriptableObject.Facing.OnRight:
                speakerAnimator.gameObject.transform.localPosition = new Vector2(speakerAnimator.gameObject.transform.position.x + animationOffset, speakerAnimator.gameObject.transform.localPosition.y);
                previousFacing = DialogueExchangeScriptableObject.Facing.OnRight;
                break;
            default:
                break;
        }
    }

        private void UpdateAnimatorOffset(float width, float height)
    {
        //Because of the width-based UI scaling, animations become closer to the center of the screen in smaller displays and further away in larger displays.
        //The switch statement below adjusts the animationOffset value to keep the animations in a more consistent position relative to the center of the screen.
        switch (width)
        {
            case float n when width<400f:
                animationOffset = width * 0.6f;
                break;
            case float n when width>=400f && width<800:
                animationOffset = width * 0.25f;
                break;
            case float n when width>=800f && width<1500:
                animationOffset = width * 0.15f;
                break;
            case float n when width>=1500:
                animationOffset = width * 0.08f;
                break; 
            default:
                break;
        }
        UpdateAnimatorPosition(previousFacing);
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        OnEndDialogue?.Invoke();
        speakerAnimator.enabled = false;
        dialogueCanvas.enabled = false;
        dialogueLines.Clear();
        isExchangeOver = true;
        if (isLooping == true && wasCloseButtonPressed == false)
        {
            StartDialogue(currentDialogueExchange);
        }
        else
        {
            wasCloseButtonPressed = false;
        }
    }

    public void CloseButtonPressed()
    {
        wasCloseButtonPressed = true;
        EndDialogue();
    }

    //Looping functionality is for playtesting only. It will be removed in the final build.
    public void ToggleLooping()
    {
        if (isLooping == false)
        {
            isLooping = true;
        }
        else
        {
            isLooping = false;
        }
    }
    //

}
