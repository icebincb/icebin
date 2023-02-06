using System;
using System.Collections;
using System.Collections.Generic;
using CropPlant;
using UnityEngine;

namespace Inventory
{
    public class Item : MonoBehaviour
    {
        public int itemid;
        private SpriteRenderer _spriteRenderer;
        public ItemDetails _itemDetails;
        private BoxCollider2D _boxCollider2D;
        private void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            if(itemid!=0)
                Init(itemid);
        }

        public void Init(int id)
        {
            itemid = id;
            _itemDetails = InventoryManager.Instance.GetItemDetails(itemid);
            if (_itemDetails != null)
            {
                 _spriteRenderer.sprite = _itemDetails.itemOnWorldSprite!=null?_itemDetails.itemOnWorldSprite:_itemDetails.itemIcon;
                EventHandler.CallGetSpriteAfterInit(_itemDetails);
            }
            //修改碰撞体尺寸
            Vector2 newSize = new Vector2(_spriteRenderer.sprite.bounds.size.x, _spriteRenderer.sprite.bounds.size.y);
            _boxCollider2D.size = newSize;
            _boxCollider2D.offset = new Vector2(0,_spriteRenderer.sprite.bounds.center.y);
            if (_itemDetails.itemType == ItemType.ReapableScenery)
            {
                gameObject.AddComponent<ReapItem>();
                gameObject.GetComponent<ReapItem>().InitCropDetails(_itemDetails.itemID);
            }
            
           // Debug.Log(_spriteRenderer.sprite.bounds.center.y);
        }
    }
}

