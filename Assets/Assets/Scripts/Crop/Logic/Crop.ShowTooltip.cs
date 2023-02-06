using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CropPlant
{
    public partial class Crop 
    {
        public GameObject cropTooltip;
        private bool flag = false;
        private RectTransform cropTransformParent=>GameObject.FindWithTag("CropTooltip").GetComponent<RectTransform>();
      
        public IEnumerator SetTooltipValid( Vector3 mouseWorldPos)
        {
            GameObject currentTooltip=null;
            if (cropTransformParent.childCount > 0)
            {
                Destroy(cropTransformParent.GetChild(0).gameObject);
                currentTooltip = Instantiate(cropTooltip, mouseWorldPos, Quaternion.identity, cropTransformParent);
            }
            if(cropTransformParent.childCount == 0)
                currentTooltip = Instantiate(cropTooltip, mouseWorldPos, Quaternion.identity, cropTransformParent);
            
                
            flag = true;
            if (currentTooltip != null)
            {
                CropTooltip tooltip=currentTooltip.GetComponent<CropTooltip>();
                tooltip.SetupTooltip(cropDetails,currentTile.growthDays);
                currentTooltip.gameObject.GetComponent<RectTransform>().anchoredPosition3D = mouseWorldPos;
                currentTooltip.SetActive(true);
                yield return new WaitForSeconds(2);
                if(currentTooltip)
                    currentTooltip.SetActive(false);
            }
            
              
            
        }
    }
}