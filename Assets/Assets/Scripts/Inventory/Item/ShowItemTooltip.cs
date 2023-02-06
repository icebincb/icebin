using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
namespace Inventory
{
    [RequireComponent(typeof(SlotUI))]
    public class ShowItemTooltip : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        private SlotUI _slotUI;
        private InventoryUI _inventoryUI=>GetComponentInParent<InventoryUI>();
        private void Awake()
        {
            _slotUI = GetComponent<SlotUI>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_slotUI.itemDetails == null)
            {
                _inventoryUI.itemTooltip.gameObject.SetActive(false);
                return;
            }
                
            _inventoryUI.itemTooltip.gameObject.SetActive(true);
            _inventoryUI.itemTooltip.SetupTooltip(_slotUI.itemDetails,_slotUI.slotType);
            
            _inventoryUI.itemTooltip.transform.position = transform.position + Vector3.up * 60;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _inventoryUI.itemTooltip.gameObject.SetActive(false);
        }
    }

}
