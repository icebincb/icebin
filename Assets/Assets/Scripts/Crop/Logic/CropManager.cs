using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using UnityEngine;

namespace CropPlant
{
    public class CropManager : Singleton<CropManager>
    {
        public CropDataList_SO cropDataList;
        private Transform cropParent;
        private Grid currentGrid;
        private Season currentSeason;
        public GameObject cropDropPrefeb;
        
        private void OnEnable()
        {
            EventHandler.PlantSeedEvent += PlantSeedEvent;
            EventHandler.AfterSceneLoadEvent += AfterSceneLoadEvent;
            EventHandler.GameDayEvent += GameDayEvent;
            EventHandler.HarvestAtCropPosition += HarvestAtCropPosition;
        }

        private void OnDisable()
        {
            EventHandler.PlantSeedEvent -= PlantSeedEvent;
            EventHandler.AfterSceneLoadEvent -= AfterSceneLoadEvent;
            EventHandler.GameDayEvent -= GameDayEvent;
            EventHandler.HarvestAtCropPosition -= HarvestAtCropPosition;
        }
        

        private void HarvestAtCropPosition(int itemID,int num,Vector3 pos)
        {
            Sprite itemSprite = InventoryManager.Instance.GetItemDetails(itemID).itemOnWorldSprite;
            if (itemSprite != null)
            {
                GameObject cropInstance = Instantiate(cropDropPrefeb, pos, Quaternion.identity, cropParent);
                cropInstance.GetComponentInChildren<SpriteRenderer>().sprite =itemSprite;
                cropInstance.GetComponent<Crop>().isDropItem = true;
                cropInstance.GetComponent<Crop>().currentPos = pos; 
                cropInstance.GetComponent<Crop>().isMoveCrop = true;
                //StartCoroutine(WaitSeconds(1,cropInstance));
                
            }
        }

        private IEnumerator WaitSeconds(float time,GameObject cropInstance)
        {
            yield return new WaitForSeconds(time);
           
        }
        
        private void GameDayEvent(int day, Season season)
        {
            currentSeason = season;
        }

        private void AfterSceneLoadEvent()
        {
            currentGrid = FindObjectOfType<Grid>();
            cropParent = GameObject.FindWithTag("CropParent").transform;
        }

        private void PlantSeedEvent(int seedID, TileDetails tileDetails)
        {
            CropDetails currentCrop = GetCropDetails(seedID);
            if (currentCrop != null && SeasonAvailable(currentCrop.seasons)&& tileDetails.seedID==-1)
            {
                tileDetails.seedID = seedID;
                tileDetails.growthDays = 0;
                //显示农作物
                DisplayCropPlant(tileDetails, currentCrop);
            }
            else if(tileDetails.seedID!=-1)
            {
                //显示农作物
                DisplayCropPlant(tileDetails, currentCrop);
            }
        }
        /// <summary>
        /// 显示农作物
        /// </summary>
        /// <param name="tileDetails"></param>
        /// <param name="cropDetails"></param>
        private void DisplayCropPlant(TileDetails tileDetails, CropDetails cropDetails)
        {
            //成长阶段
            int growthStages = cropDetails.growthDays.Length;
            int currentStages = 0;
            int dayCounter = cropDetails.totalDays;
            if (tileDetails.growthDays >= dayCounter)
                currentStages = growthStages - 1;
            else
            {
                var count = 0;
                for (int i = 0; i < growthStages; i++)
                {
                    if (tileDetails.growthDays >= count)
                    {
                        count += cropDetails.growthDays[i];
                        if (tileDetails.growthDays < count)
                        {
                            currentStages = i;
                            break;
                        }
                    }
                }
            }
            
            //获取当前阶段的预制体
            GameObject cropPrefeb = cropDetails.growthPrefeb[currentStages];
            Sprite cropSprite = cropDetails.growthSprites[currentStages];
            Vector3 pos = new Vector3(tileDetails.gridx + 0.5f, tileDetails.gridy + 0.5f, 0);
            GameObject cropInstance = Instantiate(cropPrefeb, pos, Quaternion.identity, cropParent);
            cropInstance.GetComponentInChildren<SpriteRenderer>().sprite = cropSprite;
            cropInstance.GetComponent<Crop>().cropDetails = cropDetails;
            cropInstance.GetComponent<Crop>().currentTile = tileDetails;

        }
        /// <summary>
        /// 通过物品ID查找种子信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CropDetails GetCropDetails(int id)
        {
            return cropDataList.cropDetailsList.Find(e => e.seedID == id);  
        }

        private bool SeasonAvailable(Season[] seasons)
        {
            foreach (var season in seasons)
            {
                if (currentSeason == season)
                    return true;
            }

            return false;
        }
    }
}