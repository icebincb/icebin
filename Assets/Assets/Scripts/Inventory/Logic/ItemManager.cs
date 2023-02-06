using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Inventory
{
   public class ItemManager : MonoBehaviour
   {
       public Item itemPrefeb;
       public Item bouncePrefeb;
       private Transform itemParent;

       private Transform playerTransform => GameObject.FindWithTag("Player").transform;
       //记录场景Item
       public Dictionary<string, List<SceneItem>> sceneItemDict = new Dictionary<string, List<SceneItem>>();
       private void OnEnable()
       {
           EventHandler.InstantiaItemInScene+=OnInstantiaItemInScene;
           EventHandler.AfterSceneLoadEvent+=Init;
           EventHandler.BeforeSceneUnloadEvent +=BeforeSceneUnloadEvent;
           EventHandler.DropItemEvent += DropItemEvent;

       }
        private void OnDisable()
       {
           EventHandler.InstantiaItemInScene-=OnInstantiaItemInScene;
           EventHandler.BeforeSceneUnloadEvent -= BeforeSceneUnloadEvent;
           EventHandler.AfterSceneLoadEvent-=Init;
           EventHandler.DropItemEvent -= DropItemEvent;
       }

        private void DropItemEvent(int id, Vector3 pos,ItemType itemType)
        {
            if (itemType == ItemType.Seed) return;
            var item = Instantiate(bouncePrefeb, playerTransform.position, Quaternion.identity, itemParent);
            item.itemid = id;
            var dir = (pos - playerTransform.position).normalized;
            item.GetComponent<ItemBounce>().InitBounceItem(pos,dir);
        }

        private void BeforeSceneUnloadEvent()
       {
          GetAllSceneItem();
       }


       
       private void Init()
       {
           itemParent = GameObject.FindWithTag("ItemParent").transform;
           RecreatAllItems();
       }

       private void OnInstantiaItemInScene(int id, Vector3 pos)
       {
           var item = Instantiate(itemPrefeb, pos, Quaternion.identity, itemParent);
           item.itemid = id;
       }
        /// <summary>
        /// 获取场景的所有物品
        /// </summary>
       private void GetAllSceneItem()
       {
           List<SceneItem> items = new List<SceneItem>();
           foreach (var item in FindObjectsOfType<Item>())
           {
               SceneItem sceneItem = new SceneItem()
               {
                   itemID = item.itemid,
                   position = new SerializableVector3(item.transform.position)
               };
               items.Add(sceneItem);
           }
            //更新场景物品
           if (sceneItemDict.ContainsKey(SceneManager.GetActiveScene().name))
           {
               sceneItemDict[SceneManager.GetActiveScene().name] = items;
           }
           else
           {
               sceneItemDict.Add(SceneManager.GetActiveScene().name,items);
           }
       }

        public void RecreatAllItems()
        {
            List<SceneItem> currentSceneItem = new List<SceneItem>();
            if (sceneItemDict.TryGetValue(SceneManager.GetActiveScene().name, out currentSceneItem))
            {
                if (currentSceneItem != null)
                {
                    //清场
                    foreach (var item in FindObjectsOfType<Item>())
                    {
                        Destroy(item.gameObject);
                    }

                    foreach (var item in currentSceneItem)
                    {
                        Item newItem = Instantiate(itemPrefeb, item.position.ToVector3(), quaternion.identity,
                            itemParent);
                        newItem.Init(item.itemID);
                    }
                }
            }
        }
   } 
}

