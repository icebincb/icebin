using System;
using UnityEngine;

namespace Assets.Scripts.NPC.Data
{
    [System.Serializable]
    public class ScheduleDetails : IComparable<ScheduleDetails>
    {
        public int hour, minute, day;
        public int priority;//优先级越小越先执行
        public Season season;
        public string targetScene;
        public Vector2Int targetGridPosition;
        public AnimationClip animAtStop;
        public bool interactable;

        public ScheduleDetails(int hour, int minute, int day, int priority, Season season, string targetScene,
            Vector2Int targetGridPosition, AnimationClip animAtStop, bool interactable)
        {
            this.day = day;
            this.minute = minute;
            this.hour = hour;
            this.priority = priority;
            this.season = season;
            this.targetScene = targetScene;
            this.targetGridPosition = targetGridPosition;
            this.animAtStop = animAtStop;
            this.interactable = interactable;
        }

        public int time => hour * 100 + minute;
        public int CompareTo(ScheduleDetails other)
        {
            if (time == other.time)
            {
                if (priority > other.priority)
                    return 1;
                else return -1;
                
            }else if (time > other.time)
            {
                return 1;
            }
            else
            {
                return -1;
            }

            return 0;
        }
    }
}