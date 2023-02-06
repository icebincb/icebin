using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Dialogue.Data
{
    [CreateAssetMenu(fileName = "DialogueSpriteData_SO", menuName = "DialogueSpriteData/DialogueSprite", order = 0)]
    public class DialogueSpriteData_SO : ScriptableObject
    {
        public string characterName;
        public Sprite defaultSprite;
        public Sprite happySprite; 
        public Sprite supperHappySprite;
        public Sprite surpriseSprite;
        public Sprite sadSprite;
        public Sprite suspectSprite;
    }
}