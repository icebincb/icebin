using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CropPlant
{
    
    public partial class Crop : MonoBehaviour
    {
        public CropDetails cropDetails;
        private int harvestActionCount = 1;
        public bool isMoveCrop = false;
        public GameObject currentCrop;
        public Vector3 currentPos;
        private Transform playerPos;
        public bool isMoveToPlayer = false;
        public TileDetails currentTile;
        public bool isDropItem = false;
        public bool isHarvest => currentTile.growthDays >= cropDetails.totalDays;
        float x = 0;
        float y = 0;
        private Animator anim;
        private void Start()
        {
            playerPos = GameObject.FindWithTag("Player").transform;
            anim = GetComponentInChildren<Animator>();
        }


        private void Update()
        {
            if (isMoveToPlayer && isDropItem)
            {
                currentCrop = this.gameObject;
                playerPos = GameObject.FindWithTag("Player").transform;
                currentPos = transform.position;
                MoveItemToPlayer();

            }

            if (isMoveCrop && isDropItem)
            {
                currentCrop = this.gameObject;
                MoveToTarget();
                currentPos = transform.position;
            }


        }

        private void MoveItemToPlayer()
        {
            var speed = 8f;
            var targetpos = playerPos.position + Vector3.up * 0.5f;
            var distance = Vector3.Distance(currentPos, targetpos);
            var direction = (targetpos - currentPos).normalized;
            if (Vector3.Distance(currentPos, targetpos) > 0.1f)
            {

                currentCrop.transform.position += direction * speed * Time.deltaTime;

            }
            else
            {

                Destroy(currentCrop.gameObject);
                isMoveToPlayer = false;

            }

        }

        private void MoveToTarget()
        {
            var speed = 1f;
            var speedx = 0.5f;
            if (x == 0)
                x = Random.Range(currentPos.x - 0.5f, currentPos.x + 0.5f);
            if (y == 0)
                y = Random.Range(currentPos.y - 0.5f, currentPos.y + 0.5f);
            var targetpos = new Vector3(x, y, 0);

            var direction = (targetpos - currentPos).normalized;
            var directionx = new Vector3(targetpos.x - currentPos.x, 0, 0).normalized;

            if (Vector3.Distance(currentPos, targetpos) > 0.1f)
            {
                currentCrop.transform.position +=
                    direction * speed * Time.deltaTime + directionx * speedx * Time.deltaTime;
            }
            else
            {
                isMoveCrop = false;
                //isMoveToPlayer = true;
                StartCoroutine(WaitSeconds(0.5f));
            }

        }

        private IEnumerator WaitSeconds(float time)
        {
            yield return new WaitForSeconds(time);
            isMoveToPlayer = true;
        }

        public IEnumerator ProcessToolAction(ItemDetails tool, TileDetails tileDetails)
        {
            //currentTile = tileDetails;
            var pos = new Vector3(tileDetails.gridx, tileDetails.gridy, 0);
            //工具需要使用的次数
            int requireToolCount = cropDetails.GetToolRequireCount(tool.itemID);

            if (requireToolCount == -1)
               yield return null;
            //TODO:判断是否有动画

            //点击计数器
            if (harvestActionCount < requireToolCount)
            {
                harvestActionCount++;
                //TODO:播放粒子效果 声音等
                if (anim != null&& cropDetails.hasAnimation)
                {
                    if (playerPos.position.x < transform.position.x)
                    {
                        anim.SetTrigger("RotateRight");
                    }
                        
                    else anim.SetTrigger("RotateLeft");
                }
                
                if(cropDetails.hasParticalEffect)
                    EventHandler.CallParticalEffectEvent(cropDetails.effectType,transform.position+cropDetails.effectPos);
            }
            else
            {
              
                if (cropDetails.generateAtPlayerPosition)
                {
                    if (cropDetails.hasAnimation&&anim != null)
                    {
                        if (playerPos.position.x < transform.position.x)
                        {
                            anim.SetTrigger("FallingRight"); 
                        }
                        else anim.SetTrigger("FallingLeft");
                        yield return HarvestAfterAnimation(pos);
                    }
                    else
                    {
                       //生成农作物
                        SpawnCropItems(pos); 
                    }
                    
                }
                
            }

        }

        private IEnumerator HarvestAfterAnimation(Vector3 pos)
        {
            while (!anim.GetCurrentAnimatorStateInfo(0).IsName("End"))
            {
                yield return null;
            }
            //生成农作物
            SpawnCropItems(pos);
            //转换新物体
            if (cropDetails.transformItemID > 0)
            {
                CreateTansformCrop();
            }
        }

        private void CreateTansformCrop()
        {
            currentTile.seedID = cropDetails.transformItemID;
            currentTile.daySinceLastHarvest = -1;
            currentTile.growthDays = 0;
            currentTile.daySincedig = 0;
            EventHandler.CallRefreshCurrentMap();
        }
        public void SpawnCropItems(Vector3 pos)
        {
            for (int i = 0; i < cropDetails.produceItemID.Length; i++)
            {
                int cropNum = 0;
                if (cropDetails.produceMinAmount[i] <= cropDetails.produceMaxAmount[i])
                {
                    cropNum = Random.Range(cropDetails.produceMinAmount[i], cropDetails.produceMaxAmount[i] + 1);
                }
                else
                {
                    cropNum = Random.Range(cropDetails.produceMaxAmount[i], cropDetails.produceMinAmount[i] + 1);
                }


                if (cropNum > 0 && cropDetails.generateAtPlayerPosition)
                {
                    EventHandler.CallHarvestAtCropPosition(cropDetails.produceItemID[i], cropNum,
                        new Vector3(pos.x + 0.5f, pos.y + 0.5f*(i+1), 0));
                }

            }

            RefreshCropStatus();

        }

        private void RefreshCropStatus()
        {

            if (currentTile != null)
            {
                currentTile.daySinceLastHarvest++;
                //是否可以重复生长
                if (cropDetails.daysToRegrow > 0 && currentTile.daySinceLastHarvest < cropDetails.regrowTimes)
                {
                    currentTile.growthDays = cropDetails.totalDays - cropDetails.daysToRegrow;
                    //刷新种子

                }
                else //不可重复生长
                {
                    currentTile.daySinceLastHarvest = -1;
                    currentTile.seedID = -1;
                    currentTile.daySincedig = -1;
                    currentTile.canDig = true;
                }

                EventHandler.CallRefreshCurrentMap();
                
            }
        }
    }
}