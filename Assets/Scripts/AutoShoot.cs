using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AimMode
{
    NotAim = 0,
    AimPlayer = 1
}

public class AutoShoot : MonoBehaviour
{
    // 射击频率
    public float shootFrequency = 3f;
    public AimMode aimMode = AimMode.AimPlayer;
    public Vector3 shootDirection = Vector3.left;
    // 可瞄准的最大角度
    public float minAimAngle = 0;
    public float maxAimAngle = 180;
    // 原定瞄准敌人各个角度的Sprite
    public Sprite[] aimSprites;

    // 玩家GameObject 用于MoveMode.Situ
    private GameObject vicViper;
    private SpriteRenderer sr;
    private float aimAngle = 0;
    private float halfRemainder = 0;
    private float angleMVXFlatAngle = 0;
    private float angleMVXWeekAngle = 0;
    private int currentSpriteIdx = 0;


    private float deltaTime = 0;
    private Spaceship spaceshipScript;

    // Start is called before the first frame update
    void Start()
    {
        vicViper = GameObject.FindGameObjectWithTag("VicViper");
        sr = GetComponent<SpriteRenderer>();
        spaceshipScript = GetComponent<Spaceship>();
        aimAngle = maxAimAngle - minAimAngle;
        halfRemainder = (360 - aimAngle) / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if(Tool.IsOutOfCameraX(gameObject.transform.position.x, -gameObject.transform.localScale.x * .5f))
        {
            return;
        }
        switch (aimMode)
        {
            case AimMode.AimPlayer:
                Aim();
                Shoot();
                break;
            case AimMode.NotAim:
                Shoot(shootDirection);
                break;
            default:
                break;
        }
    }

    private void Aim()
    {
        if (vicViper == null)
        {
            return;
        }
        float eachAngle = aimAngle / aimSprites.Length;
        // 当前对象到vicViper的向量
        Vector3 mv = vicViper.transform.position - transform.position;
        // mv与x轴正方向的夹角
        float signedAngle = Vector3.SignedAngle(Quaternion.Euler(0, 0, minAimAngle) * Vector3.right * transform.localScale.x * transform.localScale.y, mv, Vector3.forward * transform.localScale.x);
        angleMVXFlatAngle = signedAngle;
        // 转换为0~360
        angleMVXWeekAngle = angleMVXFlatAngle < 0 ? 360 + angleMVXFlatAngle : angleMVXFlatAngle;
        // 设定该使用的Sprite
        currentSpriteIdx = Mathf.Clamp((int)((angleMVXWeekAngle - eachAngle / 2) / eachAngle), 0, aimSprites.Length - 1);
        if (angleMVXFlatAngle >= minAimAngle - halfRemainder && angleMVXFlatAngle < minAimAngle)
        {
            currentSpriteIdx = 0;
        }
        else if (angleMVXWeekAngle > maxAimAngle && angleMVXFlatAngle < maxAimAngle + halfRemainder)
        {
            currentSpriteIdx = aimSprites.Length - 1;
        }
        sr.sprite = aimSprites[currentSpriteIdx];
    }

    private void Shoot()
    {
        // 当前对象发射点到vicViper的向量
        Vector3 bv = vicViper.transform.position - spaceshipScript.barrel.transform.position;
        Vector3 direction = bv;
        
        if (angleMVXFlatAngle >= minAimAngle - halfRemainder && angleMVXFlatAngle < minAimAngle)
        {
            direction = Quaternion.Euler(0, 0, minAimAngle * transform.localScale.x) * Vector3.right * transform.localScale.x * transform.localScale.y;
        }
        else if (angleMVXWeekAngle > maxAimAngle && angleMVXFlatAngle < maxAimAngle + halfRemainder)
        {
            direction = Quaternion.Euler(0, 0, maxAimAngle * transform.localScale.x) * Vector3.right * transform.localScale.x * transform.localScale.y;
        }

        Shoot(direction);
    }

    private void Shoot(Vector3 direction)
    {
        deltaTime += Time.deltaTime;
        if (spaceshipScript == null || !spaceshipScript.canShoot || deltaTime < shootFrequency || vicViper == null)
        {
            return;
        }

        spaceshipScript.Shoot(direction, spaceshipScript.barrel.transform.position, Quaternion.identity);
        // 重置计时
        deltaTime = 0;
    }
}
