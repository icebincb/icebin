using System.Collections.Generic;
using Assets.Scripts.Quest;
using UnityEngine;

namespace Assets.Scripts.Dialogue.Data
{
    [System.Serializable]
    public class Dialogue_Piece
    {
        public string dialogueID;
        public Sprite image;
        public int targetIndex;
        [TextArea]
        public string text;
        public QuestData_SO questData;
        public CharacterSpriteType NpcSpriteType;
        public CharacterSpriteType PlayerSpriteType;
        public List<Dialogue_Option> dialogueOptionList = new List<Dialogue_Option>();
    }
}