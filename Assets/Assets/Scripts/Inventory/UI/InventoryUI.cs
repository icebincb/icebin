using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class InventoryUI : MonoBehaviour
{

   public ItemTooltip itemTooltip; 
   [Header("背包UI")] 
   [SerializeField] private GameObject bagUI;

   public Image dragImage;
   private bool bagOpended;
   private bool isEnterDialogue=false;
   [SerializeField]private SlotUI[] _slotUis;

   private void OnEnable()
   {
      EventHandler.EnterDialogue += EnterDialogue;
      EventHandler.ExitDialogue += ExitDialogue;
      EventHandler.UpdateInventoryUI+=OnUpdateInventoryUI;
      EventHandler.BeforeSceneUnloadEvent += BeforeSceneUnloadEvent;
   }
   
   private void OnDisable()
   {
      EventHandler.EnterDialogue -= EnterDialogue;
      EventHandler.ExitDialogue -= ExitDialogue;
      EventHandler.UpdateInventoryUI-=OnUpdateInventoryUI;
      EventHandler.BeforeSceneUnloadEvent -= BeforeSceneUnloadEvent;
   }
   private void ExitDialogue()
   {
      isEnterDialogue = false;
   }

   private void EnterDialogue()
   {
      isEnterDialogue = true;
   }
   private void BeforeSceneUnloadEvent()
   {
      UpdateSlotHighLight(-1);
      bagUI.SetActive(false);
   }

   private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> items)
   {
      switch (location)
      {
         case InventoryLocation.Box:
            break;
         case InventoryLocation.Player:
           
            for (int i = 0; i < items.Count; i++)
            {
               if (items[i].ItemNum > 0)
               {
                  var item = InventoryManager.Instance.GetItemDetails(items[i].ItemID);
                  _slotUis[i].UpdateSlot(item,items[i].ItemNum);
               }else
                  _slotUis[i].UpdateEmptySlot();
            }
            break;
         default: break;
      }
   }
   private void Start()
   {
      for (int i = 0; i < _slotUis.Length; i++)
      {
         _slotUis[i].index = i;
      }

      bagOpended = bagUI.activeInHierarchy;
   }

   private void Update()
   {
      if(!isEnterDialogue)
         if (Input.GetKeyDown(KeyCode.B)||Input.GetKeyDown(KeyCode.Escape))
         {
            OpenBagUI();
         }
   }

   public void OpenBagUI()
   {
      bagOpended = !bagOpended;
      bagUI.SetActive(bagOpended);
   }

   public void UpdateSlotHighLight(int index)
   {
      foreach (var slot in _slotUis)
      {
         if (slot.isSelected && slot.index == index)
         {
            slot.highLight.gameObject.SetActive(true);
         }
         else
         {
            slot.isSelected = false;
            slot.highLight.gameObject.SetActive(false);
         }
      }
   }
}
