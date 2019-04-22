using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PrimaryWeaponType
{
    Normal = 0,
    Double = 1,
    Laser = 2
}
public class VicViper : Spaceship
{
    #region 出生相关属性
    public GameObject spawnPoint;
    public GameObject freePoint;
    [Header("移动到FreePoint后的无敌时间")]
    public float invincibleTime = 1f;

    private float lastTwinklingTime = 0f;
    // 闪烁频率
    private float twinklingF = .1f;
    // 登场移动速度修正
    private float comeOnSpeedRevise = .7f;
    private bool isComeOnScene = false;
    private float arrivalSceneTime = -1f;
    #endregion 出生相关属性
    public string VICVIPER_PATH = "Prefabs/Vicviper/Vicviper";
    // 复活机会
    public int revival = 1;
    public int basePower = 0;
    public KeyCode shootKey = KeyCode.J;
    public KeyCode usePower = KeyCode.K;
    
    public const int missileIdx = 1;
    public int maxMissileLevel = 1;
    public int baseMissileLevel = 0;

    public string OPTION_PATH = "Prefabs/Vicviper/Option";
    public int maxOptionNum = 3;
    private int baseOptionNum = 0;
    private GameObject optionPre;
    
    public Vector3 halfLocalScale { get; private set; }

    public PrimaryWeaponType primaryWeapon { get; private set; } = PrimaryWeaponType.Normal;
    public int currentMissileNum { get; private set; } = 0;

    public GameObject[] myOptions { get; private set; }
    public int currentOptionNum { get; private set; } = 0;
    
    public int currentPower { get; private set; } = 0;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        Relive();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDie)
        {
            return;
        }
        if (isComeOnScene)
        {
            ComeOnScene();
        }
        else
        {
            Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (Input.GetKeyDown(shootKey))
            {
                Shoot();
            }
            if (Input.GetKeyDown(usePower))
            {
                GroupUp();
            }
        }
        if(isInvincible && arrivalSceneTime>0 && Time.time - arrivalSceneTime >= invincibleTime)
        {
            isInvincible = false;
            spriteR.enabled = true;
            arrivalSceneTime = 0;
        }
        else if(isInvincible)
        {
            Twinkling();
        }
    }

    #region Init Function
    protected override void Init()
    {
        base.Init();
        halfLocalScale = transform.localScale * .5f;
        myOptions = new GameObject[maxOptionNum];
    }

    protected override void InitCurrentAttr()
    {
        base.InitCurrentAttr();
        currentPower = basePower;
        currentMissileNum = baseMissileLevel;
        currentOptionNum = baseOptionNum;
        primaryWeapon = PrimaryWeaponType.Normal;
    }

    protected override void InitGameObjects()
    {
        base.InitGameObjects();
        optionPre = Resources.Load<GameObject>(OPTION_PATH);
        LoadOptions();
    }

    private void LoadOptions()
    {
        //GameObject optionPre = Resources.Load<GameObject>(OPTION_PATH);
        //myOptions = new GameObject[maxOptionNum];
        //Option optionScript = optionPre.GetComponent<Option>();
        //for (int i = 0; i < maxOptionNum; i++)
        //{
        //    if(i <= 0)
        //    {
        //        optionScript.followTarget = gameObject;
        //    }
        //    else
        //    {
        //        optionScript.followTarget = myOptions[i - 1];
        //    }
        //    //options[i] = Instantiate(optionPre, transform.position, Quaternion.identity, transform);
        //    myOptions[i] = Instantiate(optionPre, transform.position, Quaternion.identity, transform.parent);
        //    myOptions[i].SetActive(false);
        //}
    }
    #endregion Init Function

    protected override void Move(float horizontal, float vertical)
    {
        Vector3 recordPos = gameObject.transform.position;
        base.Move(horizontal, vertical);

        float x = gameObject.transform.position.x;
        float y = gameObject.transform.position.y;
        if (Tool.IsOutOfCameraX(x, halfLocalScale.x))
        {
            x = recordPos.x;
        }
        if (Tool.IsOutOfCameraY(y, halfLocalScale.y))
        {
            y = recordPos.y;
        }
        gameObject.transform.position = new Vector3(x, y, 0);
    }

    #region Shoot Function
    protected void Shoot()
    {
        if(primaryWeapon == PrimaryWeaponType.Normal || primaryWeapon == PrimaryWeaponType.Laser)
        {
            NormalShoot();
        }

        if(primaryWeapon == PrimaryWeaponType.Double)
        {
            DoubleShoot();
        }

        if (currentMissileNum > 0)
        {
            MissileShoot();
        }
    }

    private void NormalShoot()
    {
        Shoot(Vector3.right, barrel.transform.position, Quaternion.identity);
        for (int i = 0; i < currentOptionNum; i++)
        {
            Instantiate(bullets[bulletIdx], myOptions[i].transform.position + myOptions[i].transform.right * .5f, Quaternion.identity);
        }
    }

    private void DoubleShoot()
    {
        NormalShoot();

        Shoot(Vector3.right, transform.position + (transform.right+transform.up)*.5f, Quaternion.Euler(0, 0, 45));
        for (int i = 0; i < currentOptionNum; i++)
        {
            Instantiate(bullets[bulletIdx], myOptions[i].transform.position + (myOptions[i].transform.right+ myOptions[i].transform.up) * .5f, Quaternion.Euler(0, 0, 45));
        }
    }

    private void MissileShoot()
    {
        int memoIdx = bulletIdx;
        bulletIdx = missileIdx;
        Shoot(Vector3.right, transform.position + (transform.right - transform.up) * .5f, Quaternion.Euler(0, 0, 315));
        for (int i = 0; i < currentOptionNum; i++)
        {
            Instantiate(bullets[missileIdx], myOptions[i].transform.position+ (myOptions[i].transform.right - myOptions[i].transform.up) * .5f, Quaternion.Euler(0, 0, 315));
        }
        bulletIdx = memoIdx;
    }
    #endregion Shoot Function

    #region GroupUp Function
    private void GroupUp()
    {
        if (currentPower <= 0)
        {
            return;
        }
        bool groupUpSuccess = false;
        switch (currentPower)
        {
            case 1:
                groupUpSuccess = PowerUpSpeedUp();
                break;
            case 2:
                groupUpSuccess = PowerUpMissile();
                break;
            case 3:
                groupUpSuccess = PowerUpDouble();
                break;
            case 4:
                groupUpSuccess = PowerUpLaser();
                break;
            case 5:
                groupUpSuccess = PowerUpOption();
                break;
            //case 6:
            //    groupUpSuccess = GetForce();
            //    break;

        }
        if (groupUpSuccess)
        {
            currentPower = 0;
        }
    }

    private bool PowerUpSpeedUp()
    {
        bool ret = false;

        if(currentSpeed <= maxSpeed)
        {
            currentSpeed++;
            ret = true;
        }
        return ret;
    }

    private bool PowerUpMissile()
    {
        if (currentMissileNum >= maxMissileLevel)
        {
            return false;
        }
        currentMissileNum++;
        return true;
    }

    private bool PowerUpDouble()
    {
        if(primaryWeapon == PrimaryWeaponType.Double)
        {
            return false;
        }
        primaryWeapon = PrimaryWeaponType.Double;
        return true;
    }

    private bool PowerUpLaser()
    {
        if(primaryWeapon == PrimaryWeaponType.Laser)
        {
            return false;
        }
        primaryWeapon = PrimaryWeaponType.Laser;
        bulletIdx = (int)primaryWeapon;
        return true;
    }

    #region Get Lost Option
    private bool PowerUpOption()
    {
        if(currentOptionNum >= maxOptionNum)
        {
            return false;
        }

        int currentOptionIdx = currentOptionNum;
        if (currentOptionNum <= 0)
        {
            optionPre.GetComponent<Option>().followTarget = gameObject;
        }
        else
        {
            optionPre.GetComponent<Option>().followTarget = myOptions[currentOptionIdx - 1];
        }
        myOptions[currentOptionIdx] = Instantiate(optionPre, transform.position, Quaternion.identity, transform);
        currentOptionNum++;
        return true;
    }

    private void LostOptions()
    {
        for(int i = 0; i < currentOptionNum; i++)
        {
            //myOptions[i].GetComponent<Option>().LostMaster(transform.parent);
            myOptions[i].GetComponent<Option>().LostMaster(null);
        }
        currentOptionNum = 0;
    }
    #endregion Get Lost Option

    private bool PowerUpForce()
    {
        return false;
    }
    #endregion GroupUp Function

    #region Spawn And Relive Function
    private void Relive()
    {
        isInvincible = true;
        // 重置属性
        //Init();
        InitCurrentAttr();
        // 移动到出生点
        //float cameraHalfWidth = Tool.GetCameraScale().x * .5f;
        //Vector3 initPosition = new Vector3(Tool.GetCamera().transform.position.x - cameraHalfWidth + halfLocalScale.x, 0, 0);
        //transform.position = initPosition;
        //gameObject.SetActive(true);
        transform.position = spawnPoint.transform.position;
        isComeOnScene = true;
        coll.enabled = true;
    }

    private void ComeOnScene()
    {
        transform.position = Vector3.MoveTowards(transform.position, freePoint.transform.position, currentSpeed * comeOnSpeedRevise * Time.deltaTime);
        isComeOnScene = !(transform.position == freePoint.transform.position);
        if (!isComeOnScene)
        {
            arrivalSceneTime = Time.time;
        }
    }

    private void Twinkling()
    {
        if (Time.time - lastTwinklingTime > twinklingF)
        {
            spriteR.enabled = !spriteR.enabled;
            lastTwinklingTime = Time.time;
        }
    }

    #endregion Spawn And Relive Function

    #region Hurt Function
    public override void Hurt(int damage)
    {
        if (damage <= 0)
        {
            return;
        }
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Die(false);
            LostOptions();
        }
        revival--;
        if (revival > 0)
        {
            Relive();
        }
    }
    #endregion Hurt Function

    #region Public Function
    public void GetPowerBox()
    {
        currentPower++;
        if(currentPower > 5)
        {
            currentPower = 1;
        }
    }

    public void GetRevivalBox()
    {
        revival++;
    }
    #endregion Public Function

    #region Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if("Ground".Equals(collision.tag))
        {
            Hurt(currentHp);
        }
        else if ("Option".Equals(collision.tag) && currentOptionNum < maxOptionNum)
        {
            if (currentOptionNum < maxOptionNum)
            {
                int currentOptionIdx = currentOptionNum;
                if (currentOptionNum <= 0)
                {
                    collision.GetComponent<Option>().PickedUp(transform, gameObject);
                }
                else
                {
                    collision.GetComponent<Option>().PickedUp(transform, myOptions[currentOptionIdx - 1]);
                }
                myOptions[currentOptionIdx] = collision.gameObject;
                currentOptionNum++;
            }
        }
    }
    #endregion Trigger

    #region OnDrawGizmos
    private void OnDrawGizmos()
    {
        DrawSpawnPoint();
        DrawFreePoint();
        DrawLineSpawnToFree();
    }

    private void DrawSpawnPoint()
    {
        if (spawnPoint)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(spawnPoint.transform.position, .1f);
        }
    }

    private void DrawFreePoint()
    {
        if (freePoint)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(freePoint.transform.position, .1f);
        }
    }

    private void DrawLineSpawnToFree()
    {
        if (freePoint && spawnPoint)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(spawnPoint.transform.position, freePoint.transform.position);
        }
    }
    #endregion OnDrawGizmos
}
