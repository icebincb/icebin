using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName="ItemList_SO",menuName = "Inventory/ItemDataList")]
public class ItemList_SO :ScriptableObject
{
    public List<ItemDetails> item;
}
