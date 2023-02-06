using System.Collections;
using System.Collections.Generic;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CropTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [SerializeField] private ItemList_SO _itemListSo;

    public void SetupTooltip(CropDetails cropDetails, int growthDays)
    {
        foreach (var item in _itemListSo.item)
        {
            if (item.itemID == cropDetails.seedID)
            {
                 nameText.text =item.itemname;
                 break;
            }
        }
        typeText.text ="已成长"+growthDays+"天";

        descriptionText.text ="总共需要成长的天数："+ cropDetails.totalDays;
        
        //强制立即渲染
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}
