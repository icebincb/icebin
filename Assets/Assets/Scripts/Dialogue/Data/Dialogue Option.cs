using UnityEngine;

namespace Assets.Scripts.Dialogue.Data
{
    [System.Serializable]
    public class Dialogue_Option
    {
        public string text;
        public int targetDialogueIndex;
        public bool taskQuest;
    }
}