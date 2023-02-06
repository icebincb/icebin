using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.PlayerLoop;

public class SwitchBounds : MonoBehaviour
{
    

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += Switch;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= Switch;
    }

    private void Switch()
    {
        PolygonCollider2D polygonCollider2D =
            GameObject.FindGameObjectWithTag("Bounds").GetComponent<PolygonCollider2D>();
        CinemachineConfiner cinemachineConfiner = GetComponent<CinemachineConfiner >();
        cinemachineConfiner.m_BoundingShape2D = polygonCollider2D;
        //每一次切换需要清除缓存
        cinemachineConfiner.InvalidatePathCache();
    }


}
