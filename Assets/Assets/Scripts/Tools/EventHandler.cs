using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler
{

   public static event Action<InventoryLocation, List<InventoryItem>> UpdateInventoryUI;

    public static void CallUpdateInventoryUI( List<InventoryItem> item, InventoryLocation location)
    {
        UpdateInventoryUI?.Invoke(location,item);
    }

    public static event Action<int, Vector3> InstantiaItemInScene;

    public static void CallInstantiaItemInScene(int id, Vector3 pos)
    {
        InstantiaItemInScene?.Invoke(id,pos);
    }
    public static event Action<int, Vector3,ItemType> DropItemEvent;

    public static void CallDropItemEvent(int id, Vector3 pos,ItemType itemType)
    {
        DropItemEvent?.Invoke(id,pos,itemType);
    }
    public static event Action<ItemDetails, bool> ItemSelectedEvent;

    public static void CallItemSelectedEvent(ItemDetails itemDetails, bool isSelected)
    {
        ItemSelectedEvent?.Invoke(itemDetails,isSelected);
    }

    public static event Action<int, int,int,Season> GameMinuteEvent;

    public static void CallGameMinuteEvent(int min, int hour,int day,Season season)
    {
        GameMinuteEvent?.Invoke(min,hour,day,season);
    }
    public static event Action<int, int, int, int, Season> GameDateEvent;

    public static void CallGameDateEvent(int hour, int day, int month, int year, Season season)
    {
        GameDateEvent?.Invoke(hour,day,month,year,season);
    }

    public static event Action<int, Season> GameDayEvent;

    public static void CallGameDayEvent(int day, Season season)
    {
        GameDayEvent?.Invoke(day,season);
    }

    public static event Action<string, Vector3> TransitionEvent;

    public static void CallTransitionEvent(string sceneToGo, Vector3 positionToGo)
    {
        TransitionEvent?.Invoke(sceneToGo,positionToGo);
    }

    public static event Action BeforeSceneUnloadEvent;

    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }

    public static event Action AfterSceneLoadEvent;

    public static void CallAfterSceneLoadEvent()
    {
        AfterSceneLoadEvent?.Invoke();
    }

    public static event Action<Vector3> MoveToPosition;

    public static void CallMoveToPosition(Vector3 targetPosition)
    {
        MoveToPosition?.Invoke(targetPosition);
    }

    public static event Action<Vector3, ItemDetails> MouseClickedEvent;

    public static void CallMouseClickedEvent(Vector3 pos, ItemDetails itemDetails)
    {
        MouseClickedEvent?.Invoke(pos,itemDetails);
    }
    public static event Action<Vector3, ItemDetails> ExcuteActionAfterAnimation;

    public static void CallExcuteActionAfterAnimation(Vector3 pos, ItemDetails itemDetails)
    {
        ExcuteActionAfterAnimation?.Invoke(pos,itemDetails);
    }
    public static event Action<ItemDetails> GetSpriteAfterInit;

    public static void CallGetSpriteAfterInit(ItemDetails itemDetails)
    {
        GetSpriteAfterInit?.Invoke(itemDetails);
    }

    public static event Action<int, TileDetails> PlantSeedEvent;

    public static void CallPlantSeedEvent(int seedID, TileDetails tileDetails)
    {
        PlantSeedEvent?.Invoke(seedID,tileDetails);
    }

    public static event Action<int,int,Vector3> HarvestAtCropPosition;

    public static void CallHarvestAtCropPosition(int id,int num,Vector3 pos)
    {
        HarvestAtCropPosition?.Invoke(id,num,pos);
    }

    public static event Action RefreshCurrentMap;

    public static void CallRefreshCurrentMap()
    {
        RefreshCurrentMap?.Invoke();
    }

    public static event Action<ParticaleEffectType, Vector3> ParticalEffectEvent;

    public static void CallParticalEffectEvent(ParticaleEffectType particaleEffectType, Vector3 pos)
    {
        ParticalEffectEvent?.Invoke(particaleEffectType,pos);
    }

    public static event Action CropGenerator;

    public static void CallCropGenerator()
    {
        CropGenerator?.Invoke();
    }

    public static event Action<int> DialogueStatueUpdate;

    public static void CallDialogueStatueUpdate(int index)
    {
        DialogueStatueUpdate?.Invoke(index);
    }

    public static event Action EnterDialogue;

    public static void CallEnterDialogue()
    {
        EnterDialogue?.Invoke();
    }

    public static event Action ExitDialogue;

    public static void CallExitDialogue()
    {
        ExitDialogue?.Invoke();
    }
}
