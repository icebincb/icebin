using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using UnityEngine;

public class AnimatorOverride : MonoBehaviour
{
    private Animator[] animators;
    public SpriteRenderer holdItem;
    [Header("各部分动画列表")] public List<AnimatorType> animatorTypes;

    private Dictionary<string, Animator> animatorNameDict = new Dictionary<string, Animator>();
    private void Awake()
    {
        animators = GetComponentsInChildren<Animator>();

        foreach (var anim in animators)
        {
            animatorNameDict.Add(anim.name, anim);
        }
    }

    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent += BeforeSceneUnloadEvent;

    }

    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= OnItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent -= BeforeSceneUnloadEvent;
      
    }

    

    
    private void BeforeSceneUnloadEvent()
    {
        holdItem.enabled = false;
        SwitchAnimator(PartType.None);
        
    }

    private void OnItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        //TODO:不同的工具动画在这里切换 需要后面补全
        PartType currentType = itemDetails.itemType switch
        {
            ItemType.Seed=>PartType.Carry,
            ItemType.Commodity=>PartType.Carry,
            ItemType.HoeTool=>PartType.Hoe,
            ItemType.Furniture=>PartType.Carry,
            ItemType.ChopTool=>PartType.Chop,
            ItemType.BreakTool=>PartType.Break,
            ItemType.WaterTool=>PartType.Water,
            ItemType.CollectTool=>PartType.Collect,
            ItemType.ReapTool=>PartType.Reap,
            _ => PartType.None,
        };
        if (isSelected == false)
        {
            currentType = PartType.None;
            holdItem.enabled = false;
        }
        else
        {
            if (currentType == PartType.Carry)
            {
                holdItem.sprite = itemDetails.itemOnWorldSprite;
                holdItem.enabled = true;
            }
            else
            {
                holdItem.enabled = false;
            }
        }
        SwitchAnimator(currentType);
    }

    private void SwitchAnimator(PartType partType)
    {
        foreach (var anim in animatorTypes)
        {
            if (anim.partType == partType)
            {
                animatorNameDict[anim.partName.ToString()].runtimeAnimatorController = anim.animatorOverrideController;
            }
        }
    }
}
