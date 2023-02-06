using System;
using Map;
using UnityEngine;
namespace CropPlant
{
    public class CropGenerator: MonoBehaviour
    {
        private Grid currentGrid;
        public int seedItemID;
        public int growthDays;

        private void Awake()
        {
            currentGrid = FindObjectOfType<Grid>();
        }

        private void OnEnable()
        {
            EventHandler.CropGenerator += GenerateCrop;
        }

        private void OnDisable()
        {
            EventHandler.CropGenerator -= GenerateCrop;
        }

        private void GenerateCrop()
        {
            Vector3Int cropPos = currentGrid.WorldToCell(transform.position);
           
            if (seedItemID != 0)
            {
                var tile = GridMapManager.Instance.GetTileDetailsOnMousePosition(cropPos);
                if (tile == null)
                {
                    tile = new TileDetails();
                }
                tile.gridx = cropPos.x;
                tile.gridy = cropPos.y;
                tile.daySincewatered = -1;
                tile.canDig=false;
                tile.canDropItem = false;
                tile.canPlace = false;
                tile.seedID = seedItemID;
                tile.growthDays = growthDays;
                GridMapManager.Instance.UpdateTileMap(tile);
            }
        }
    }
}