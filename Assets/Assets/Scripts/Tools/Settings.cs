using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    public const float fadeDuration = 0.35f;
    public const float targetAlpha = 0.45f;
    
    //时间相关
    public const float secondThreshold = 0.12f;
    public const int secondHold = 59;
    public const int hourHold = 23;
    public const int minuteHold = 59;
    public const int dayHold = 30;
    public const int seasonHold=3;

    public const float fadeTimeLimit = 1.5f;
    public const float gridCellSize = 1f;
    public const float gridCellDiagonalSize = 1.414f;

    public const float pixelSize = 0.05f;//20*20 占一个格子
    public const float animationBreakTime=5f;
    public const int maxGridSize = 9999;

    public const int dialogueMaxSize = 100;

}
