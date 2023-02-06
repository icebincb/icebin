using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.NPC.Data
{
    [CreateAssetMenu(fileName = "SceneRouteData_SO", menuName = "NPCRouteData/NPCRouteDataList", order = 0)]
    public class SceneRouteData_SO : ScriptableObject
    {
        public List<SceneRoute> sceneRouteList;
    }
}