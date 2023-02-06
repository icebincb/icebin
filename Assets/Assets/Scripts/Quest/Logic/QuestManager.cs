using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Quest.Logic
{
    public class QuestManager : Singleton<QuestManager>
    {
        public List<QuestTask> questTaskList;

        public bool HaveQuest(QuestData_SO data)
        {
            if (data != null)
                return questTaskList.Any(q => q.questData.questName == data.questName); 
            return false;
        }

        public QuestTask GetTask(QuestData_SO data)
        {
            return questTaskList.Find(q => q.questData.questName == data.questName);
        }
    }
}