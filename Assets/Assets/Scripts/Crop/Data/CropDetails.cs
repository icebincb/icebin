using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CropDetails
{
    public int seedID;
    [Header("不同阶段需要的天数")] public int[] growthDays;
    
    public int totalDays
    {
        get
        {
            int amount = 0;
            foreach (var days in growthDays)
            {
                amount += days;
            }

            return amount;
        }
    }

    [Header("不同阶段的Prefeb")] public GameObject[] growthPrefeb;

    [Header("不同阶段的图片")] public Sprite[] growthSprites;
    [Header("可种植的季节")] public Season[] seasons;
    [Space] [Header("收割工具")] public int[] harvestToolItemID;
    [Header("每种工具使用次数")] public int[] requireActionCount;
    [Header("转换ID")] public int transformItemID;
    [Space] [Header("收割果实信息")] 
    public int[] produceItemID;
    public int[] produceMaxAmount;
    public int[] produceMinAmount;
    public Vector2 spawnRadius;
    [Header("再次生长的时间")] public int daysToRegrow;
    public int regrowTimes;
    [Header("options")] public bool generateAtPlayerPosition;
    public bool hasAnimation;
    public bool hasParticalEffect;
    //TODO:特效 音效等
    public ParticaleEffectType effectType;
    public Vector3 effectPos;
    /// <summary>
    /// 检查工具是否可用
    /// </summary>
    /// <param name="toolID"></param>
    /// <returns></returns>
    public bool CheckToolAvailable(int toolID)
    {
        foreach (var tool in harvestToolItemID)
        {
            if (tool == toolID)
                return true;
        }

        return false;
    }
    /// <summary>
    /// 获取工具使用次数
    /// </summary>
    /// <param name="toolID"></param>
    /// <returns></returns>
    public int GetToolRequireCount(int toolID)
    {
        int i = 0;
        foreach (var tool in harvestToolItemID)
        {
           
            if (tool == toolID)
                break;
            i++;
        }

        if (i >= requireActionCount.Length)
            return -1;
        return requireActionCount[i];
    }
}
