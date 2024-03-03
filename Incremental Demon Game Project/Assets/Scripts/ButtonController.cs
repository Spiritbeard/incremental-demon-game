using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        DialogueManager.OnStartDialogue += DisableButton;
        DialogueManager.OnEndDialogue += EnableButton;
    }

    private void OnDisable()
    {
        DialogueManager.OnStartDialogue -= DisableButton;
        DialogueManager.OnEndDialogue -= EnableButton;
    }

    private void DisableButton()
    {
        button.interactable = false;
    }

    private void EnableButton()
    {
        button.interactable = true;
    }
}
