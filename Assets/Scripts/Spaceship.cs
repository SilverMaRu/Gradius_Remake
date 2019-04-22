using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ShootPosition
{
    MySelf = 0,
    Barrel = 1
}
public class Spaceship : MonoBehaviour
{
    public Team team = Team.leftTeam;
    public int maxHp = 1;
    public int baseHp = 1;
    public int maxSpeed = 10;
    public float baseSpeed = 5;
    public string bulletPath = "Prefabs/Bullets";
    [Header("是否朝向移动方向")]
    public bool orientToDirection = false;
    public string dieEffectPath = "Prefabs/Effects/Die/Normal";

    public int currentHp { get; protected set; } = 1;
    public float currentSpeed { get; protected set; } = 5;
    public GameObject barrel { get; protected set; }
    public GameObject[] bullets { get; protected set; }
    public int bulletIdx { get; protected set; } = 0;
    public bool canShoot { get; protected set; } = false;
    public bool isInvincible { get; protected set; } = false;
    public bool isDie { get; protected set; } = false;

    //protected Rigidbody2D rb2D;
    protected SpriteRenderer spriteR;
    protected Collider2D coll;
    //private float invincibleStartTime;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Spaceship spaceship = collision.GetComponent<Spaceship>();
        if(spaceship != null && !spaceship.isInvincible && spaceship.team != team)
        {
            spaceship.Hurt(1);
        }
    }

    protected virtual void Init()
    {
        InitCurrentAttr();
        InitGameObjects();
        InitComponents();
    }

    protected virtual void InitCurrentAttr()
    {
        currentHp = baseHp;
        currentSpeed = baseSpeed;
        bulletIdx = 0;
        isDie = false;
    }

    protected virtual void LoadBullets()
    {
        if(bullets == null)
        {
            bullets = Resources.LoadAll<GameObject>(bulletPath);
        }
        canShoot = bullets != null;
    }

    protected virtual void FindBarrel()
    {
        Transform barrelTransform = transform.Find("Barrel");
        if(barrelTransform == null)
        {
            barrel = null;
        }
        else
        {
            canShoot = true;
            barrel = barrelTransform.gameObject;
        }
    }

    protected virtual void InitGameObjects()
    {
        LoadBullets();
        FindBarrel();
    }

    protected virtual void InitComponents()
    {
        //rb2D = gameObject.GetComponent<Rigidbody2D>();
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        coll = gameObject.GetComponent<Collider2D>();
    }

    protected virtual void Move(float horizontal, float vertical)
    {
        Vector3 deltaDis = (Vector3.up * vertical + Vector3.right * horizontal) * currentSpeed * Time.deltaTime;
        gameObject.transform.Translate(deltaDis);
    }

    public void Shoot(Vector3 direction, Vector3 position, Quaternion rotation)
    {
        if (!canShoot)
        {
            return;
        }
        Bullet bulletScript = bullets[bulletIdx].GetComponent<Bullet>();
        bulletScript.shootTeam = team;
        bulletScript.direction = direction;
        bullets[bulletIdx].transform.localScale = transform.localScale;
        Instantiate(bullets[bulletIdx], position, rotation);
    }

    public virtual void Hurt(int damage)
    {
        if(damage <= 0)
        {
            return;
        }
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        Die(true);
    }

    protected void Die(bool doDestroy)
    {
        GameObject dieEffect = Resources.Load<GameObject>(dieEffectPath);
        dieEffect.transform.localScale = transform.localScale;
        Instantiate(dieEffect, transform.position, Quaternion.identity);
        if (doDestroy)
        {
            Destroy(gameObject);
        }
        else
        {
            //gameObject.SetActive(false);
            isDie = true;
            spriteR.enabled = false;
            coll.enabled = false;
        }
        Reward rewardScript = gameObject.GetComponent<Reward>();
        if (rewardScript != null)
        {
            rewardScript.BuildAward();
        }
    }
}
