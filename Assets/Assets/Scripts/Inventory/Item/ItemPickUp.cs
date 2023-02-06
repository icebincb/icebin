using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using UnityEngine;

namespace Inventory
{
    public class ItemPickUp : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Item item = other.GetComponent<Item>();
           // Debug.Log(item);
            if (item != null)
            {
                if (item._itemDetails.canPickedup)
                {
                    //拾取物品
                    InventoryManager.Instance.AddItem(item,true);
                }
            }
        }
    }
}

