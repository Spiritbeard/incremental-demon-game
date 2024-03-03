using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActivitiesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] activityManagers;
    private int currentActiveActivity;
    private bool isDialogueActive;

    private void Awake()
    {
        currentActiveActivity = 0;
        SetActiveActivity(0);
    }

    private void OnEnable()
    {
        DialogueManager.OnStartDialogue += SetDialogueActiveTrue;
        DialogueManager.OnEndDialogue += SetDialogueActiveFalse;
    }

    private void OnDisable()
    {
        DialogueManager.OnStartDialogue -= SetDialogueActiveTrue;
        DialogueManager.OnEndDialogue -= SetDialogueActiveFalse;
    }

    public void SetActiveActivity(int newActiveActivity)
    {
        foreach (GameObject gameObject in activityManagers)
        {
            gameObject.SetActive(false);
        }

        if (isDialogueActive != true)
        {
            if (newActiveActivity != 0 && newActiveActivity != currentActiveActivity)
            {
                activityManagers[newActiveActivity-1].SetActive(true);
                currentActiveActivity = newActiveActivity;
            }
            else
            {
                currentActiveActivity = 0;
            }
        }
    }

    private void SetDialogueActiveTrue()
    {
        isDialogueActive = true;
        SetActiveActivity(0);
    }

    private void SetDialogueActiveFalse()
    {
        isDialogueActive = false;
    }
}
