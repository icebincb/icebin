using System;
using System.Collections;
using System.Collections.Generic;
using CropPlant;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace Map
{


    public class GridMapManager : Singleton<GridMapManager>
    {
        [Header("地图信息")] public List<MapData_SO> mapdataList;

        [Header("种地瓦片切换信息")] 
        public RuleTile digTile;
        public RuleTile waterTile;

        private Tilemap digMap;
        private Tilemap waterMap;

        private Season currentSeason;
        //场景名字+坐标 获取对应的信息
        private Dictionary<string, TileDetails> _tileDetailsMap = new Dictionary<string, TileDetails>();
        private Dictionary<string, bool> firstLoadScene = new Dictionary<string, bool>();
        private Grid currentGrid;

        private List<ReapItem> itemsInRadius;
        private void Start()
        {
            foreach (var map in mapdataList)
            {
                firstLoadScene.Add(map.sceneName,true);
                InittileDetailsMap(map);
            }
        }

        private void OnEnable()
        {
           EventHandler.ExcuteActionAfterAnimation+=ExcuteActionAfterAnimation;
           EventHandler.AfterSceneLoadEvent += AfterSceneLoadEvent;
           EventHandler.GameDayEvent += GameDayEvent;
           EventHandler.RefreshCurrentMap += RefreshMap;
        }

        


        private void OnDisable()
        {
            EventHandler.ExcuteActionAfterAnimation-=ExcuteActionAfterAnimation;
            EventHandler.AfterSceneLoadEvent -= AfterSceneLoadEvent;
            EventHandler.GameDayEvent -= GameDayEvent;
            EventHandler.RefreshCurrentMap -= RefreshMap;
        }
        /// <summary>
        /// 每天调用一次
        /// </summary>
        /// <param name="day"></param>
        /// <param name="season"></param>
        private void GameDayEvent(int day, Season season)
        {
            currentSeason = season;

            foreach (var tile in _tileDetailsMap)
            {
                if (tile.Value.daySincewatered > -1)
                {
                    tile.Value.daySincewatered++;
                }

                if (tile.Value.daySincewatered > 2)
                {
                    tile.Value.daySincewatered=-1;
                   
                } 

                if (tile.Value.seedID > -1)
                {
                    tile.Value.growthDays++;
                }
                else
                {
                    tile.Value.growthDays = 0;
                }
            }
            RefreshMap();
        }

        private void AfterSceneLoadEvent()
        {
            currentGrid = FindObjectOfType<Grid>();
            digMap = GameObject.FindWithTag("Dig").GetComponent<Tilemap>();
            waterMap= GameObject.FindWithTag("Water").GetComponent<Tilemap>();
            //DisplayMap(SceneManager.GetActiveScene().name);

            if (firstLoadScene[SceneManager.GetActiveScene().name])
            {
                EventHandler.CallCropGenerator();
                firstLoadScene[SceneManager.GetActiveScene().name] = false;
            }
                
            RefreshMap();
        }
        private void InittileDetailsMap(MapData_SO mapData)
        {
            foreach (var item in mapData.tilePropertys)
            {
                TileDetails temp = new TileDetails
                {
                    gridx = item.tileCoordinate.x,
                    gridy = item.tileCoordinate.y
                };
                string key = item.tileCoordinate.x + "x" + item.tileCoordinate.y + "y" + mapData.sceneName;
                if (GetTileDetails(key) != null)
                {
                    temp = GetTileDetails(key);
                }

                switch (item.girdType)
                {
                    case GirdType.Diggable:
                        temp.canDig = item.boolTypeValue;
                        break;
                    case GirdType.DropItem:
                        temp.canDropItem = item.boolTypeValue;
                        break;
                    case GirdType.PlaceFurniture:
                        temp.canPlace = item.boolTypeValue;
                        break;
                    case GirdType.NPCObstacle:
                        temp.isNPCObstacle = item.boolTypeValue;
                        break;
                }

                if (GetTileDetails(key) != null)
                    _tileDetailsMap[key] = temp;
                else _tileDetailsMap.Add(key, temp);
            }


        }


        public TileDetails GetTileDetailsOnMousePosition(Vector3Int mousePosition)
        {
            string key = mousePosition.x + "x" + mousePosition.y + "y" + SceneManager.GetActiveScene().name;
            return GetTileDetails(key);
        }

        public TileDetails GetTileDetails(string key)
        {
            if (_tileDetailsMap.ContainsKey(key))
                return _tileDetailsMap[key];
            return null;
        }
        /// <summary>
        /// 执行实际物品或工具
        /// </summary>
        /// <param name="mouseWorldPos"></param>
        /// <param name="itemDetails"></param>
        private void ExcuteActionAfterAnimation(Vector3 mouseWorldPos, ItemDetails itemDetails)
        {
            var mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);
            var currentTile = GetTileDetailsOnMousePosition(mouseGridPos);
            if (currentTile != null)
            {
                //TODO:物品使用实际功能
                switch (itemDetails.itemType)
                {
                        case ItemType.Commodity :
                            EventHandler.CallDropItemEvent(itemDetails.itemID,mouseWorldPos,ItemType.Commodity);
                            break;
                        case ItemType.Seed:
                            EventHandler.CallPlantSeedEvent(itemDetails.itemID,currentTile);
                            EventHandler.CallDropItemEvent(itemDetails.itemID,mouseWorldPos,ItemType.Seed);
                            break;
                        case ItemType.Furniture: break;
                        case ItemType.BreakTool: 
                        case ItemType.ChopTool: 
                        case ItemType.CollectTool:
                            Crop currentCrop = GetCropObject(mouseWorldPos);
                            if(currentCrop!=null)
                            //执行收割方法
                            StartCoroutine(currentCrop.ProcessToolAction(itemDetails,currentTile)) ;
                            break;
                        case ItemType.HoeTool:
                            SetDigGround(currentTile);
                            currentTile.daySincedig = 0;
                            currentTile.canDig = false;
                            currentTile.canDropItem = false;
                            
                            break;
                        case ItemType.ReapableScenery: break;
                        case ItemType.ReapTool:
                          
                            for (int i = 0; i < itemsInRadius.Count; i++)
                            {
                                if (itemsInRadius[i] == null)
                                    continue;
                                
                                EventHandler.CallParticalEffectEvent(ParticaleEffectType.ReapableScenery,itemsInRadius[i].transform.position+Vector3.up);
                                itemsInRadius[i].SpawnCropItems();
                                Destroy(itemsInRadius[i].gameObject);
                            }
                            break;
                        case ItemType.WaterTool:
                            SetWaterGround(currentTile);
                            currentTile.daySincewatered = 0;
                            break;
                }
            }
            UpdateTileMap(currentTile);
        }
        /// <summary>
        /// 通过鼠标点击 触发碰撞体判断是否点击到农作物
        /// </summary>
        /// <param name="mouseWorldpos"></param>
        /// <returns></returns>
        public Crop GetCropObject(Vector3 mouseWorldpos)
        {
            Collider2D[] collider2Ds = Physics2D.OverlapPointAll(mouseWorldpos);
            for (int i = 0; i < collider2Ds.Length; i++)
            {
                if (collider2Ds[i].GetComponent<Crop>())
                {
                    return collider2Ds[i].GetComponent<Crop>();
                }
            }

            return null;
        }

        public bool HaveReapableItemsInRadius(Vector3 mouseWorldPos,ItemDetails tool)
        {
            itemsInRadius = new List<ReapItem>();
            Collider2D[] collider2Ds = new Collider2D[20];
            Physics2D.OverlapCircleNonAlloc(mouseWorldPos, tool.itemUseRadius-1, collider2Ds);
          
            if (collider2Ds.Length > 0)
            {
                for (int i = 0; i < collider2Ds.Length; i++)
                {
                    if (collider2Ds[i] != null)
                    {
                        var item = collider2Ds[i].GetComponent<ReapItem>();
                        //Debug.Log(item.name);
                        if(item!=null)
                            itemsInRadius.Add(item);
                    }
                }
            }
            
            return itemsInRadius.Count > 0;
        }
        //显示挖坑瓦片
        private void SetDigGround(TileDetails tile)
        {
            Vector3Int pos = new Vector3Int(tile.gridx, tile.gridy, 0);
            if (digMap != null)
            {
                digMap.SetTile(pos,digTile);
            }
        }
        //显示浇水瓦片 
        private void SetWaterGround(TileDetails tile)
        {
            Vector3Int pos = new Vector3Int(tile.gridx, tile.gridy, 0);
            if (waterMap != null)
            {
                waterMap.SetTile(pos,waterTile);
            }
        }
        /// <summary>
        /// 更新瓦片信息
        /// </summary>
        /// <param name="tileDetails"></param>
        public void UpdateTileMap(TileDetails tileDetails)
        {
            string key = tileDetails.gridx + "x" + tileDetails.gridy + "y" + SceneManager.GetActiveScene().name;
            if (_tileDetailsMap.ContainsKey(key))
            {
                _tileDetailsMap[key] = tileDetails;
            }
            else
            {
                _tileDetailsMap.Add(key,tileDetails);
            }
        }
        /// <summary>
        /// 刷新地图
        /// </summary>
        private void RefreshMap()
        {
            if(digMap!=null)
                digMap.ClearAllTiles();
            if(waterMap!=null)
                digMap.ClearAllTiles();
            foreach (var crop in FindObjectsOfType<Crop>())
            {
                if(crop.isDropItem==false)
                    Destroy(crop.gameObject);
            }
            
            DisplayMap(SceneManager.GetActiveScene().name);
        }
        private void DisplayMap(string sceneName)
        {
            foreach (var tile in _tileDetailsMap)
            {
                var key = tile.Key;
                var tileDetails = tile.Value;
                if (key.Contains(sceneName))
                {
                    if(tileDetails.daySincedig>-1)
                        SetDigGround(tileDetails);
                    if(tileDetails.daySincewatered>-1)
                        SetWaterGround(tileDetails);
                    //种子
                    if (tileDetails.seedID > -1)
                    {
                        EventHandler.CallPlantSeedEvent(tileDetails.seedID,tileDetails);
                    }
                }
            }
        }
        /// <summary>
        /// 获取当前地图节点的信息
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="girdDimensions"></param>
        /// <param name="gridOrigin"></param>
        /// <returns></returns>
        public bool GetMapNodeData(string sceneName,out Vector2Int girdDimensions,out Vector2Int gridOrigin)
        {
            girdDimensions=Vector2Int.zero;
            gridOrigin=Vector2Int.zero;
            foreach (var map in mapdataList)
            {
                if (map.sceneName == sceneName)
                {
                    girdDimensions = new Vector2Int(map.gridWidth, map.gridHeight);
                    gridOrigin = new Vector2Int(map.originX, map.originY);
                    return true;
                    
                }
            }

            return false;
        }
    }
}