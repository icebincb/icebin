using System.Collections.Generic;
using Assets.Scripts.NPC.Data;
using UnityEngine;

namespace Assets.Scripts.NPC
{
    public class NPCManager : Singleton<NPCManager>
    {
        public List<NPCPosition> npcPositions;
        public SceneRouteData_SO sceneRouteData;
        private Dictionary<string, SceneRoute> sceneRouteDic = new Dictionary<string, SceneRoute>();
        protected override void Awake()
        {
            base.Awake();
            InitSceneRouteDic();
        }

        private void InitSceneRouteDic()
        {
            if (sceneRouteData.sceneRouteList.Count > 0)
            {
                foreach (var sceneRoute in sceneRouteData.sceneRouteList)
                {
                    var key = sceneRoute.fromSceneName + sceneRoute.gotoSceneName;
                    if (sceneRouteDic.ContainsKey(key)) continue;
                    else
                    {
                        sceneRouteDic.Add(key,sceneRoute);
                    }
                }
            }
        }

        public SceneRoute GetSceneRoute(string from, string to)
        {
            var key = from + to;
            if (sceneRouteDic.ContainsKey(key))
                return sceneRouteDic[key];
            return null;
        }
    }
}