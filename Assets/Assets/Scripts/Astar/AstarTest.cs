using System;
using System.Collections.Generic;
using Assets.Scripts.NPC;
using Assets.Scripts.NPC.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Astar
{
    public class AstarTest : MonoBehaviour
    {
        private Astar _astar;
        [Header("用于测试")] public Vector2Int startPos,endPos;
        public Tilemap displaymap;
        public TileBase displaytile;
        public bool displayAstarTest;
        public bool displayPath;
        private Stack<MovementStep> npcPath;
        
        [Header("测试移动")] public NPCMovement npcMovement;
        public bool moveNpc;
        [SceneName]public string testTargetScene;
        public Vector2Int targetPos;
        public AnimationClip animationClip;
        private void Awake()
        {
            _astar = GetComponent<Astar>();

            npcPath = new Stack<MovementStep>();
        }

        private void Update()
        {
            ShowPath();
            if (moveNpc)
            {
                moveNpc = false;
                var schedule = new ScheduleDetails(0, 0, 0, 0, Season.春天, testTargetScene, targetPos, animationClip,
                    true);
                npcMovement.BuildPath(schedule);
            }
        }

        private void ShowPath()
        {
            if (displaymap != null && displaytile != null)
            {
                if (displayAstarTest)
                {
                    displaymap.SetTile((Vector3Int)startPos,displaytile);
                    displaymap.SetTile((Vector3Int)endPos,displaytile);
                }
                else
                {
                    displaymap.SetTile((Vector3Int)startPos,null);
                    displaymap.SetTile((Vector3Int)endPos,null);
                }

                if (displayPath)
                {
                    var sceneName = SceneManager.GetActiveScene().name;
                    _astar.BuildPath(sceneName,startPos,endPos,npcPath);
                    foreach (var step in npcPath)
                    {
                        displaymap.SetTile((Vector3Int)step.gridPos,displaytile);
                    }
                }
                else
                {       
                    if (npcPath.Count > 0)
                    {
                        foreach (var step in npcPath)
                        {
                            displaymap.SetTile((Vector3Int)step.gridPos,null);
                        }
                        npcPath.Clear();
                    }
                }
                
            }
        }
    }
}