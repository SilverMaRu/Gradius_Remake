using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Team
{
    leftTeam = 1,
    neutral = 0,
    rightTeam = -1
}

public class Bullet : MonoBehaviour
{
    public Team shootTeam = Team.leftTeam;
    public bool isMissile = false;
    public bool isHitLimited = true;
    public int maxHitNum = 1;
    public Vector3 direction = Vector3.right;
    public float speed = 15f;
    public int firePower = 1;
    
    private float limitMoveDistance = 0;
    private int currentHitNum = 0;
    // 导弹贴地面走时极限修正角度
    private float missileLimitAngle = 90;
    private bool isCloseToGround = false;

    // Start is called before the first frame update
    void Start()
    {
        direction.Normalize();
        SetLimitDistance();
    }

    // Update is called once per frame
    void Update()
    {
        BulletMove();
        BulletAutoDestroy();
    }

    private void SetLimitDistance()
    {
        limitMoveDistance = Tool.GetCameraScale().x * .5f;
    }

    private void BulletMove()
    {
        Vector3 deltaDis = direction * speed * Time.deltaTime;
        if(isMissile)
        {
            if (HitGroundCheck(missileLimitAngle))
            {
                Destroy(gameObject);
            }
            else
            {
                MissileReviseDirection();
            }
        }
        transform.Translate(deltaDis);
    }

    private bool HitGroundCheck(float limitAngle)
    {
        bool ret = true;
        RaycastHit2D hitGroundHit = Physics2D.Raycast(transform.position, transform.right, .2f, LayerMask.GetMask("Ground"));

        if(hitGroundHit.transform == null)
        {
            return false;
        }
        //// 导弹前方与法线的夹角
        float angleSN = Vector3.Angle(transform.up, hitGroundHit.normal);
        ret = angleSN >= limitAngle;

        return ret;
    }

    private void MissileReviseDirection()
    {
        RaycastHit2D preferredHit = Physics2D.Raycast(transform.position, -transform.up, .3f, LayerMask.GetMask("Ground"));
        RaycastHit2D backupHit = Physics2D.Raycast(transform.position, Vector3.down, 1f, LayerMask.GetMask("Ground"));

        if (!isCloseToGround && preferredHit.transform == null)
        {
            return;
        }
        else if (!isCloseToGround && preferredHit.transform != null)
        {
            isCloseToGround = !isCloseToGround;
        }
        
        if (isCloseToGround && preferredHit.transform != null)
        {
            transform.up = preferredHit.normal;
        }
        else if(isCloseToGround && preferredHit.transform == null && backupHit.transform != null)
        {
            transform.up = backupHit.normal;
        }
    }

    private void BulletAutoDestroy()
    {
        // 子弹与镜头的水平差值
        if (Tool.IsOutOfCameraX(gameObject.transform.position.x))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HitGround(collision);
        HitEnemy(collision);
    }

    private void HitEnemy(Collider2D collision)
    {
        Spaceship spaceship = collision.GetComponent<Spaceship>();
        if (spaceship != null && !spaceship.isInvincible && shootTeam != spaceship.team)
        {
            spaceship.Hurt(firePower);
            if (isHitLimited)
            {
                AddHitNum();
            }
        }
    }

    private void HitGround(Collider2D collision)
    {
        if ((!isMissile || isHitLimited) && "Ground".Equals(collision.tag))
        {
            AddHitNum();
        }
    }

    public void AddHitNum()
    {
        currentHitNum++;
        if (isHitLimited && currentHitNum >= maxHitNum)
        {
            Destroy(gameObject);
        }
    }
}
