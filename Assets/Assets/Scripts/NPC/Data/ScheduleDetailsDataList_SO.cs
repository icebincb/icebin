using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.NPC.Data
{
    [CreateAssetMenu(fileName = "ScheduleDetailsDataList_SO", menuName = "NPC Schedule/ScheduleDataList")]
    public class ScheduleDetailsDataList_SO : ScriptableObject
    {
        public List<ScheduleDetails> scheduleList;
    }
}