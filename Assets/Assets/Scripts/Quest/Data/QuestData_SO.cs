using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Quest
{
    [CreateAssetMenu(fileName = "QuestData_SO", menuName = "Quest/QuestData", order = 0)]
    public class QuestData_SO : ScriptableObject
    {
        public string questName;
        [TextArea] public string description;
        public bool isStarted;
        public bool isCompleted;
        public bool isFinished;
        public List<QuestRequire> questRequireList;
        public List<QuestReward> questRewardList;
    }
}