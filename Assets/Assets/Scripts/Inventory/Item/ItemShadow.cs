using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ItemShadow : MonoBehaviour
    {
        public SpriteRenderer itemSprite;
        private SpriteRenderer shadowSpriteRenderer;

        private void Awake()
        {
            shadowSpriteRenderer = GetComponent<SpriteRenderer>(); 
        }
        private void OnEnable()
        {
            EventHandler.GetSpriteAfterInit += GetSpriteAfterInit;
        }

        private void OnDisable()
        {
            EventHandler.GetSpriteAfterInit -= GetSpriteAfterInit;
        }

        private void GetSpriteAfterInit(ItemDetails _itemDetails)
        {
           itemSprite.sprite=_itemDetails.itemOnWorldSprite!=null?_itemDetails.itemOnWorldSprite:_itemDetails.itemIcon;
           shadowSpriteRenderer.sprite = itemSprite.sprite;
           shadowSpriteRenderer.color = new Color(0, 0, 0, 0.3f);
        }
        
    }
}