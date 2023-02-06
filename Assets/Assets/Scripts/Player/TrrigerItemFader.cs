using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrrigerItemFader : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        ItemFader[] faders = other.GetComponentsInChildren<ItemFader>();
        if (faders.Length > 0)
        {
            foreach (var itemFader in faders)
            {
                itemFader.FadeOut();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ItemFader[] faders = other.GetComponentsInChildren<ItemFader>();
        if (faders.Length > 0)
        {
            foreach (var itemFader in faders)
            {
                itemFader.FadeIn();
            }
        }
    }
}
