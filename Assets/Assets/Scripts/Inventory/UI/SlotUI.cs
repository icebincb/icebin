using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


    public class SlotUI : MonoBehaviour,IPointerClickHandler,IDragHandler,IEndDragHandler,IBeginDragHandler
    {
        [Header("组件获取")] [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] public Image highLight;
        [SerializeField] private Button _button;
        [Header("格子类型")] public SlotType slotType;

        public ItemDetails itemDetails;
        public int itemAmount;
        public bool isSelected;
        public int index;
        public InventoryUI _inventoryUI=>GetComponentInParent<InventoryUI>();
        private void Start()
        {
            isSelected = false;
            if (itemDetails==null)
            {
                UpdateEmptySlot();
            }

        }

        //将格子更新为空
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                 isSelected = false;
                 _inventoryUI.UpdateSlotHighLight(-1);
                 EventHandler.CallItemSelectedEvent(itemDetails,isSelected);
            }

            itemDetails = null;
            slotImage.enabled = false;
            amountText.text = string.Empty;
            _button.interactable = false;

        }

        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotImage.sprite = item.itemIcon;
            slotImage.enabled = true;
            itemAmount = amount;
            amountText.text = amount.ToString();
            _button.interactable = true;
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            if(itemDetails==null) return;
            
            isSelected = !isSelected;
           _inventoryUI.UpdateSlotHighLight(index);
           if (slotType == SlotType.Bag)
           {
               EventHandler.CallItemSelectedEvent(itemDetails,isSelected);
           }
            
        }

        public void OnDrag(PointerEventData eventData)
        {
            _inventoryUI.dragImage.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _inventoryUI.dragImage.enabled = false;
            //Debug.Log(eventData.pointerCurrentRaycast.gameObject);
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if(eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>()==null)
                    return;

                var target = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();
                int targetindex = target.index;

                if (slotType == SlotType.Bag && target.slotType == SlotType.Bag)
                {
                    InventoryManager.Instance.SwapItem(index,targetindex);
                }
                
                //清空所有高亮
                _inventoryUI.UpdateSlotHighLight(-1);
            }
           /* else//测试扔再地上
            {
                if (itemDetails.canDropped)
                {
                      //鼠标对应世界地图得坐标
                      var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                                        -Camera.main.transform.position.z));
                      EventHandler.CallInstantiaItemInScene(itemDetails.itemID,pos);
                }
              
            }*/
            
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemDetails!= null)
            {
                _inventoryUI.dragImage.enabled = true;
                _inventoryUI.dragImage.sprite = slotImage.sprite;
                _inventoryUI.dragImage.SetNativeSize();
                isSelected = true;
                _inventoryUI.UpdateSlotHighLight(index);
            }
        }
    }
