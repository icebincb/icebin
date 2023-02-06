using Map;
using UnityEngine;

namespace CropPlant
{
    public class ReapItem:MonoBehaviour
    {
        private CropDetails cropDetails;
        Grid currentGrid => FindObjectOfType<Grid>();
        private Vector3Int pos;
        TileDetails currentTile ;
        public void InitCropDetails(int id)
        {
            pos= currentGrid.WorldToCell(transform.position);
            currentTile = GridMapManager.Instance.GetTileDetailsOnMousePosition(pos);
            if (currentTile != null)
            {
                currentTile.canDig = false;
                currentTile.canPlace = false; 
                currentTile.canDropItem = false;
            }
            
            cropDetails = CropManager.Instance.GetCropDetails(id);
          
        }
        public void SpawnCropItems()
        {
            for (int i = 0; i < cropDetails.produceItemID.Length; i++)
            {
                int cropNum = 0;
                if (cropDetails.produceMinAmount[i] <= cropDetails.produceMaxAmount[i])
                {
                    cropNum = Random.Range(cropDetails.produceMinAmount[i], cropDetails.produceMaxAmount[i] + 1);
                }
                else
                {
                    cropNum = Random.Range(cropDetails.produceMaxAmount[i], cropDetails.produceMinAmount[i] + 1);
                }


                if (cropNum > 0 && cropDetails.generateAtPlayerPosition)
                {
                    EventHandler.CallHarvestAtCropPosition(cropDetails.produceItemID[i], cropNum,
                        new Vector3(pos.x + 0.5f, pos.y + 0.5f*(i+1), 0));
                }

            }

            RefreshCropStatus();
        }

        private void RefreshCropStatus()
        {
           
            if (currentTile != null)
            {
               
                currentTile.daySinceLastHarvest = -1;
                currentTile.seedID = -1;
                currentTile.daySincedig = -1; 
                currentTile.canDig = true;
                EventHandler.CallRefreshCurrentMap();
                
            }
        }
    }
}