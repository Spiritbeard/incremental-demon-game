using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoTagSystem : MonoBehaviour
{
    [SerializeField] private TMP_StyleSheet styleSheet;
    [SerializeField] private KeywordsToTag keywordsToTags;

    public string SetAutoTags(string textBoxText)
    {
        foreach (var keyword in keywordsToTags.Keywords)
        {
            if (textBoxText.Contains(keyword))
            {
                return textBoxText.Replace($"{keyword}", $"<style=\"{keyword}\">{keyword}</style>");
            }                
        }
        return textBoxText;
    }
}
