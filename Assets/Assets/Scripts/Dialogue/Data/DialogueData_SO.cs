using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Dialogue.Data
{
    [CreateAssetMenu(fileName = "DialogueData_SO", menuName = "Dialogue/dialogueData")]
    public class DialogueData_SO : ScriptableObject
    {
        public List<Dialogue_Piece> dialoguePieceList ;
    }
}