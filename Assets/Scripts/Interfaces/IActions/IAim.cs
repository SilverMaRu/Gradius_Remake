using UnityEngine;

public interface IAim
{
    // 可瞄准的最大角度
    float minAimAngle { get; set; }
    float maxAimAngle { get; set; }
    // 原定瞄准敌人各个角度的Sprite
    Sprite[] aimSprites { get; set; }

    float aimAngle { get; set; }
    float halfRemainder { get; set; }
    float angleMVXFlatAngle { get; set; }
    float angleMVXWeekAngle { get; set; }
    int currentSpriteIdx { get; set; }

    void Aim();
}
