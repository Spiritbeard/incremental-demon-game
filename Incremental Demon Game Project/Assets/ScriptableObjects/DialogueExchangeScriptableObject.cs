using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "DialogueExchange", menuName = "ScriptableObjects/DialogueExchange")]

public class DialogueExchangeScriptableObject : ScriptableObject
{

    public enum Facing{
        OnLeft,
        InCenter,
        OnRight
    }

    public Line[] lines;

    [System.Serializable]
    public struct Line
    {
        public string speakerName;
        public AnimationClip speakerAnimation;
        public LocalizedString lineText;
        public Facing speakerFacing;
    }
}
