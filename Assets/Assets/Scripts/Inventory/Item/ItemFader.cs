using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ItemFader : MonoBehaviour
{
   private SpriteRenderer _spriteRenderer;
   private void Awake()
   {
      _spriteRenderer = GetComponent<SpriteRenderer>();
   }

   //逐渐恢复颜色
   public void FadeIn()
   {
      Color color = new Color(1,1,1,1);
      _spriteRenderer.DOColor(color, Settings.fadeDuration);
   }
   
   //逐渐透明
   public void FadeOut()
   {
      Color color = new Color(1,1,1,Settings.targetAlpha);
      _spriteRenderer.DOColor(color, Settings.fadeDuration);
   }
}
