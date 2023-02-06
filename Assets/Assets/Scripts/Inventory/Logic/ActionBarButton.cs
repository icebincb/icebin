using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [RequireComponent(typeof(SlotUI))]
    public class ActionBarButton : MonoBehaviour
    {
        public KeyCode keyCode;
        private SlotUI slotUI;
        private bool isEnterDialogue=false;
        private void OnEnable()
        {
          
            EventHandler.EnterDialogue += EnterDialogue;
            EventHandler.ExitDialogue += ExitDialogue;
        }

        private void OnDisable()
        {
     
            EventHandler.EnterDialogue -= EnterDialogue;
            EventHandler.ExitDialogue -= ExitDialogue;
        }

        private void ExitDialogue()
        {
            isEnterDialogue = false;
        }

        private void EnterDialogue()
        {
            isEnterDialogue = true;
        }
        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(keyCode)&&!isEnterDialogue)
            {
                if (slotUI.itemDetails != null)
                {
                    slotUI.isSelected = !slotUI.isSelected;
                    if (slotUI.isSelected)
                    {
                        slotUI._inventoryUI.UpdateSlotHighLight(slotUI.index);
                    }
                    else
                    {
                        slotUI._inventoryUI.UpdateSlotHighLight(-1);
                    }
                    EventHandler.CallItemSelectedEvent(slotUI.itemDetails,slotUI.isSelected);
                }
            }
        }
    }
}