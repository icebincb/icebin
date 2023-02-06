using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
   public class InventoryManager : Singleton<InventoryManager>
   {
       [Header("物品数据")]
       public ItemList_SO _itemListSo;
       [Header("背包数据")]
       public InventoryBag_SO playerbag;

       private void Start()
       {
           EventHandler.CallUpdateInventoryUI(playerbag.itemlist,InventoryLocation.Player);
          /* var item = new InventoryItem {ItemID =1003, ItemNum =1}; 
           Debug.Log(item.ItemNum+" "+item.ItemID);
           
           playerbag.itemlist[1] = item;*/
       }

       private void OnEnable()
       {
           EventHandler.DropItemEvent += DropItemEvent;
           EventHandler.HarvestAtCropPosition += HarvestAtCropPosition;
       }

       private void OnDisable()
       {
           EventHandler.DropItemEvent -= DropItemEvent;
           EventHandler.HarvestAtCropPosition -= HarvestAtCropPosition;
       }

       private void HarvestAtCropPosition(int itemID,int num,Vector3 pos)
       {
          AddItem(itemID,num);
       }

       private void DropItemEvent(int id, Vector3 pos,ItemType itemType)
       {
          RemoveItem(id,1);
       }

       public ItemDetails GetItemDetails(int id)
       {
           return _itemListSo.item.Find(i => i.itemID == id);
       }
        
       public void AddItem(int itemid,int num)
       {
           var index = GetIndex(itemid);

           AddItemAtIndex(itemid,index,num);

           //更新UI
           EventHandler.CallUpdateInventoryUI(playerbag.itemlist,InventoryLocation.Player);
       }
       public void AddItem(Item item,bool toDestory)
       {
           var index = GetIndex(item.itemid);

          AddItemAtIndex(item.itemid,index,1);
           InventoryItem newItem = new InventoryItem();
           newItem.ItemNum = 1;
           newItem.ItemID = item.itemid;

           //playerbag.itemlist[0] = newItem;
           if (toDestory)
           {
               Destroy(item.gameObject);
           }
           //更新UI
           EventHandler.CallUpdateInventoryUI(playerbag.itemlist,InventoryLocation.Player);
       }
        /// <summary>
        /// 移除指定数量的背包物品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="removeCount"></param>
       private void RemoveItem(int id,int removeCount)
       {
           var index = GetIndex(id);
            
           if (playerbag.itemlist[index].ItemNum > removeCount)
           {
               InventoryItem newItem = new InventoryItem();
               newItem.ItemNum = playerbag.itemlist[index].ItemNum-removeCount;
               newItem.ItemID = id;
               playerbag.itemlist[index] = newItem;
           }
           else if(playerbag.itemlist[index].ItemNum == removeCount)
           {
               InventoryItem newItem = new InventoryItem();
               playerbag.itemlist[index] = newItem;
           }
           //更新UI
           EventHandler.CallUpdateInventoryUI(playerbag.itemlist,InventoryLocation.Player);
       }
       private bool CheckBag()
       {
           foreach (var item in playerbag.itemlist)
           {
               if (item.ItemID == 0)
               {
                   return true;
               }

               
           }

           return false;
           
       }

       private int GetIndex(int id)
       {
           for (int i=0;i< playerbag.itemlist.Count;i++)
           {
               if (playerbag.itemlist[i].ItemID == id)
               {
                   return i;
               }

           }

           return -1;
       }

       private void AddItemAtIndex(int id, int index, int amount)
       {
           if (index == -1)
           {
               var item = new InventoryItem {ItemID =id, ItemNum =amount};
               for (int i = 0; i < playerbag.itemlist.Count; i++)
               {
                   if (playerbag.itemlist[i].ItemID == 0)
                   {
                       playerbag.itemlist[i] = item;
                       break;
                       
                   }
               }
           }
           else
           {
               int currentAmount = playerbag.itemlist[index].ItemNum+amount;
               var item = new InventoryItem {ItemID =id, ItemNum =currentAmount}; 
              
              // Debug.Log(index);
               playerbag.itemlist[index] = item;
           } 
//           Debug.Log(index);
       }

       public void SwapItem(int from, int to)
       {
           InventoryItem currentItem = playerbag.itemlist[from];
           InventoryItem targetItem = playerbag.itemlist[to];
         
           if (targetItem.ItemID != 0&&targetItem.ItemID!=currentItem.ItemID)
           {
               playerbag.itemlist[from] = targetItem;
               playerbag.itemlist[to] = currentItem;
              
           }    
           else if (targetItem.ItemID == currentItem.ItemID&&from!=to)
           {
               targetItem.ItemNum+=  currentItem.ItemNum;
               playerbag.itemlist[from] = new InventoryItem();
               playerbag.itemlist[to] = targetItem;

           }
           else if (targetItem.ItemID == currentItem.ItemID && from == to)
           {
               
           }
           else 
           {
              
               playerbag.itemlist[to] = currentItem;
               playerbag.itemlist[from] = new InventoryItem();
           }
           EventHandler.CallUpdateInventoryUI(playerbag.itemlist,InventoryLocation.Player);
       }
   } 
}

