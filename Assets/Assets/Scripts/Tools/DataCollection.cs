using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Quest;
using UnityEngine;

[System.Serializable]
public class ItemDetails
{
    public int itemID;
    public string itemname;
    public ItemType itemType;
    public Sprite itemIcon;
    public Sprite itemOnWorldSprite;
    public string itemDescription;
    public int itemUseRadius;
    public bool canPickedup;
    public bool canDropped;
    public bool canCarried;
    public int itemPrice;
    [Range(0, 1)]
    public float sellPercentage;
}
[System.Serializable]
public struct InventoryItem
{
    public int ItemID;
    public int ItemNum;
}
[System.Serializable]
public struct AnimatorType
{
    public PartType partType;
    public PartName partName;
    public AnimatorOverrideController animatorOverrideController;
}

[System.Serializable]
public class SerializableVector3
{
    public float x, y, z;
    public SerializableVector3(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)x,(int)y);
    }
    
}
[System.Serializable]
public class SceneItem
{
    public int itemID;
    public SerializableVector3 position;
}

[System.Serializable]
public class TileProperty
{
    public Vector2Int tileCoordinate;
    public bool boolTypeValue;
    public GirdType girdType;
}

[System.Serializable]
public class TileDetails
{
    public int gridx, gridy;

    public bool canDig;
    public bool canDropItem;
    public bool canPlace;
    public bool isNPCObstacle;
    public int daySincedig=-1;
    public int daySincewatered = -1;
    public int seedID=-1;
    public int growthDays = -1;
    public int daySinceLastHarvest=-1;

}

[System.Serializable]
public class NPCPosition
{
    public Transform npcTransform;
    public string startScene;
    public Vector3 position;
}
[System.Serializable]
//场景路径
public class SceneRoute
{
    public string fromSceneName;
    public string gotoSceneName;
    public List<ScenePath> scenePathList;
}
[System.Serializable]
public class ScenePath
{
    public string sceneName;
    public Vector2Int fromGridCell;
    public Vector2Int gotoGridCell;
    
}

[System.Serializable]
public class QuestRequire
{
    public string name;
    public int ID;
    public int requireAmount;
    public int currentAmount;
}

[System.Serializable]
public class QuestReward
{
    public string name;
    public int ID;
    public int rewardAmount;
}

[System.Serializable]
public class QuestTask
{
    public QuestData_SO questData;
    public bool IsStarted
    {
        get { return questData.isStarted; }
        set { questData.isStarted = value; }
    }
    public bool IsCompleted
    {
        get { return questData.isCompleted; }
        set { questData.isCompleted = value; }
    }
    public bool IsFinished
    {
        get { return questData.isFinished; }
        set { questData.isFinished = value; }
    }
}