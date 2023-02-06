using System;
using System.Collections;
using System.Collections.Generic;
using CropPlant;
using Inventory;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Map;
public class CursorManager : Singleton<CursorManager>
{
    public Sprite normal, tool, seed,commodity,dialogue;
    public Sprite currentSprite;
    private Image cursorImage;
    private GameObject cursor;
    //鼠标检测
    private Camera mainCamera;
    private Grid currentGrid;
    private Vector3 mouseWorldPos;
    private Vector3Int mouseGridPos;
    private bool cursorEnable = false;
    private bool cursorPositionValid=false;
    private ItemDetails currentItem;
    private bool isNormal = true;
    private bool isEnterDialogue=false;
    private Transform playerTransform=>GameObject.FindWithTag("Player").transform;
    private void Start()
    {
        mainCamera = Camera.main;
        cursor = GameObject.FindWithTag("Cursor");
        cursorImage= cursor.GetComponent<Image>();
        currentSprite = normal;
        SetCursorImage(currentSprite);
    }

    private void OnEnable()
    {
        EventHandler.ItemSelectedEvent += ItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent += BeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadEvent += AfterSceneLoadEvent;
        EventHandler.EnterDialogue += EnterDialogue;
        EventHandler.ExitDialogue += ExitDialogue;
    }

    private void OnDisable()
    {
        EventHandler.ItemSelectedEvent -= ItemSelectedEvent;
        EventHandler.BeforeSceneUnloadEvent -= BeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadEvent-= AfterSceneLoadEvent;
        EventHandler.EnterDialogue -= EnterDialogue;
        EventHandler.ExitDialogue -= ExitDialogue;
    }

    private void ExitDialogue()
    {
        isEnterDialogue = false;
    }

    private void EnterDialogue()
    {
        isEnterDialogue = true;
    }

    
    private void BeforeSceneUnloadEvent()
    {
        cursorEnable = false;
    }

    private void AfterSceneLoadEvent()
    {
        currentGrid = FindObjectOfType<Grid>();
      
    }


    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0) )
        {
            if(cursorPositionValid)
            //执行方法
            EventHandler.CallMouseClickedEvent(mouseWorldPos,currentItem);
            
        }
    }

    private void CheckInitCrop()
    {
        
       // Debug.LogError(currentSprite.name);
        if (Input.GetMouseButtonDown(0))
        {
          
                mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                    -mainCamera.transform.position.z));
                 var currentTile = GridMapManager.Instance.GetTileDetailsOnMousePosition(mouseGridPos);
                 if (currentTile != null)
                 {
                     Crop currentCrop = GridMapManager.Instance.GetCropObject(mouseWorldPos);
                     if (currentCrop != null && currentCrop.isDropItem == false)
                     {
                         //显示详情
                         StartCoroutine(currentCrop.SetTooltipValid(mouseWorldPos));
                     }
                 }
        }
    }

    public void CursorSwitch()
    {
        //TODO:地图上交互鼠标图片切换
        mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,-mainCamera.transform.position.z) );
        var npc = CheckIsNpc(mouseWorldPos);
       
        if (cursorImage.sprite== normal&&npc!=null)
        {
            currentSprite = dialogue;
            SetCursorImage(dialogue);
        }
        if (npc == null&& cursorImage.sprite==dialogue)
        {
            currentSprite = normal;
            SetCursorImage(normal);
            SetCursorValid();
        }

    }

    private GameObject CheckIsNpc(Vector3 mouseWorldpos)
    {
        Collider2D[] collider2Ds = Physics2D.OverlapPointAll(mouseWorldpos);
        for (int i = 0; i < collider2Ds.Length; i++)
        {
            if (collider2Ds[i].CompareTag("NPC"))
            {
                return collider2Ds[i].gameObject;
            }
        }
        return null;
    }
    private void ItemSelectedEvent(ItemDetails item, bool isSelect)
    {
        
        if (!isSelect)
        {
            currentSprite = normal;
            currentItem = null;
            cursorEnable = false;
            isNormal = true;
            return;
        }
        
        //TODO:补充
        currentSprite = item.itemType switch
        {
            ItemType.Seed=>seed,
            ItemType.ChopTool=>tool,
            ItemType.BreakTool=>tool,
            ItemType.HoeTool=>tool,
            ItemType.CollectTool=>tool,
            ItemType.WaterTool=>tool,
            ItemType.Furniture=>tool,
            ItemType.Commodity=>commodity,
            _=>normal
        };
        cursorEnable = true;
        currentItem = item;
        isNormal = false;
    }

    private void Update()
    {
        if(cursorImage==null) return;
        cursorImage.transform.position = Input.mousePosition;
        if (!InteractWithUI()&&!isEnterDialogue)
        {
            if (cursorEnable)
            {
                SetCursorImage(currentSprite);
                CheckCursorValid();
                CheckInput(); 
            }
            CursorSwitch();

        }
        else SetCursorImage(normal);

    }

    private void SetCursorImage(Sprite sprite)
    {
        
        var rect = cursor.GetComponent<RectTransform>().rect;
        cursor.GetComponent<RectTransform>().pivot =new Vector2(sprite.pivot.x/rect.width,sprite.pivot.y/rect.height) ;
        cursorImage.sprite = sprite;
        cursorImage.color = new Color(1, 1, 1, 1);
    }
    
    public void SetCursorValid()
    {
        cursorPositionValid = true;
        cursorImage.color = new Color(1, 1, 1, 1);
    }
    public void SetCursorInValid()
    {
        cursorPositionValid = false;
        cursorImage.color = new Color(1, 0, 0, 0.5f);
    }
    private void CheckCursorValid()
    {
        mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,-mainCamera.transform.position.z) );
        mouseGridPos = currentGrid.WorldToCell(mouseWorldPos);

        var playerGridPos = currentGrid.WorldToCell(playerTransform.position);
        
        //判断在使用范围内
        if (Mathf.Abs(mouseWorldPos.x - playerGridPos.x) > currentItem.itemUseRadius ||
            Mathf.Abs(mouseWorldPos.y - playerGridPos.y) > currentItem.itemUseRadius)
        {
            SetCursorInValid();
            return;
        }
        var currentTile = GridMapManager.Instance.GetTileDetailsOnMousePosition(mouseGridPos);

        if (currentTile != null)
        {
            CropDetails cropDetails = CropManager.Instance.GetCropDetails(currentTile.seedID);
            Crop currentCrop =GridMapManager.Instance.GetCropObject(mouseWorldPos);
            switch (currentItem.itemType)
            {
                case ItemType.Commodity :
                    if(currentTile.canDropItem&&currentItem.canDropped) SetCursorValid();
                    else SetCursorInValid();
                    break;
                case ItemType.Seed:
                    if (currentTile.daySincedig > -1 && currentTile.seedID == -1)
                        SetCursorValid();
                    else SetCursorInValid();
                    break;
                case ItemType.Furniture: break;
                case ItemType.BreakTool: 
                case ItemType.ChopTool:
                    if (currentCrop != null)
                    {
                        if (currentCrop.isHarvest&&currentCrop.cropDetails.CheckToolAvailable(currentItem.itemID))
                        {
                            SetCursorValid();
                        }
                        else
                        {
                            CheckInitCrop();
                            SetCursorInValid();
                        }
                    }else SetCursorInValid();
                    break;
                case ItemType.CollectTool:
                        
                    if (cropDetails != null&& cropDetails.CheckToolAvailable(currentItem.itemID))
                    {
                        if(currentTile.growthDays>=cropDetails.totalDays)SetCursorValid();
                        else
                        {
                            CheckInitCrop();
                            SetCursorInValid();
                        }
                    }else SetCursorInValid();
                    break;
                case ItemType.HoeTool:
                    if(currentTile.canDig)
                        SetCursorValid();
                    else SetCursorInValid();
                    break;
                case ItemType.ReapableScenery: break;
                case ItemType.ReapTool:
                    if (GridMapManager.Instance.HaveReapableItemsInRadius(mouseWorldPos, currentItem))
                    {
                        SetCursorValid();
                    }
                    else SetCursorInValid();
                    
                    break;
                case ItemType.WaterTool: 
                    if(currentTile.daySincedig>-1&&currentTile.daySincewatered==-1)
                        SetCursorValid();
                    else SetCursorInValid();
                    break;
            }
        }
        else SetCursorInValid();
        
    }

    
    public bool InteractWithUI()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        return false;
    }
}
