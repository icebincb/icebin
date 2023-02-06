using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class GridMap : MonoBehaviour
{
    public MapData_SO mapdata;
    public GirdType girdType;
    public Tilemap currentTilemap;

    private void OnEnable()
    {
        if (!Application.IsPlaying(this))
        {
            currentTilemap = GetComponent<Tilemap>();
            if(mapdata!=null)
                mapdata.tilePropertys.Clear();
        }
    }

    private void OnDisable()
    {
        if (!Application.IsPlaying(this))
        {
            currentTilemap = GetComponent<Tilemap>();
            UpdateTileMap();
            
#if UNITY_EDITOR
            //保存地图信息
            if(mapdata!=null)
                EditorUtility.SetDirty(mapdata);
#endif
        }
    }

    private void UpdateTileMap()
    {
        currentTilemap.CompressBounds();
        if (!Application.IsPlaying(this))
        {
            if (mapdata != null)
            {
                //左下角的tile坐标
                var startpos = currentTilemap.cellBounds.min;
                //右上角的tile坐标
                var endpos = currentTilemap.cellBounds.max;
                for (int x = startpos.x; x < endpos.x; x++)
                {
                    for (int y = startpos.y; y < endpos.y; y++)
                    {
                        TileBase tile = currentTilemap.GetTile(new Vector3Int(x, y, 0));
                        if (tile != null)
                        {
                            TileProperty tileProperty = new TileProperty
                            {
                                tileCoordinate = new Vector2Int(x, y),
                                girdType = this.girdType,
                                boolTypeValue = true
                            };
                            
                            mapdata.tilePropertys.Add(tileProperty);
                        }
                    }
                }
            }
        }
    }
}
