using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    public MoveMode moveMode = MoveMode.Route;
    // 锯齿型路线变换频率
    [Header("锯齿型路线变换频率(s)")]
    public float sawtoothFrequency = 1;

    // 正弦型路线变换频率
    [Header("正弦型路线变换频率(s)")]
    public float sineFrequency = 1;
    // 正弦型路线最高点幅度
    [Header("正弦型路线最高点幅度(倍率)")]
    public float sineAmplitude = 1;

    // 波浪型路线变换频率
    [Header("波浪型路线变换频率(s)")]
    public float billowFrequency = 1;
    // 波浪型路线最高点幅度
    [Header("波浪型路线最高点幅度(倍率)")]
    public float billowAmplitude = 1;
    // 是否归化方向向量
    public bool isNormalizedDirection = false;

    private Transform[] routePoints;
    private int pointIdx = 0;
    private Spaceship spaceship;
    private Vector3 destination;
    // 出现在镜头的时间
    private float appearTime = 0;
    private const float doublePi = Mathf.PI * 2;

    // 判断是否设置了目标点
    private bool isSetDes = false;
    // 判断是否抵达过目标点
    private bool isArrivedDes = false;

    // Start is called before the first frame update
    void Start()
    {
        spaceship = GetComponent<Spaceship>();
        SetRoutePoints();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void SetRoutePoints()
    {
        Transform parent = transform.parent;
        if (parent == null)
        {
            return;
        }
        Leader leader = parent.gameObject.GetComponent<Leader>();
        if (leader == null)
        {
            return;
        }
        routePoints = leader.GetRoutePoints();
    }

    private void Move()
    {
        if (spaceship == null || Tool.IsOutOfCameraX(gameObject.transform.position.x, -gameObject.transform.localScale.x * .5f))
        {
            return;
        }

        switch (moveMode)
        {
            case MoveMode.Route:
                if (routePoints != null && routePoints.Length > 0)
                {
                    OnRoute();
                }
                else
                {
                    AsZ();
                }
                break;
            case MoveMode.Sawtooth:
                AsSawtooth(sawtoothFrequency, isNormalizedDirection);
                break;
            case MoveMode.Sine:
                AsSine(sineFrequency, sineAmplitude, isNormalizedDirection);
                break;
            case MoveMode.Billow:
                AsBillow(billowFrequency, billowAmplitude, isNormalizedDirection);
                break;
            case MoveMode.Z:
                AsZ();
                break;
        }
        appearTime += Time.deltaTime;
    }

    private void SetDirection(Vector3 moveVector)
    {
        if (spaceship.orientToDirection)
        {
            transform.right = -moveVector;
        }
    }

    private void AsZ()
    {
        if (!isSetDes)
        {
            Vector3 cameraScale = Tool.GetCameraScale();
            Vector3 cameraPosition = Tool.GetCamera().transform.position;
            cameraPosition = Vector3.Scale(cameraPosition, Vector3.up + Vector3.right);// new Vector3(cameraPosition.x, cameraPosition.y, 0);

            // 对象到摄像机中心的向量
            Vector3 gc = cameraPosition - transform.position;
            if (gc.x >= cameraScale.x * .1f)
            {
                Vector3 unitGD = new Vector3(1, gc.y / Mathf.Abs(gc.y) * .5f, 0).normalized;
                destination = gameObject.transform.position + unitGD * gc.magnitude * 1.5f;
                isSetDes = true;
            }
        }
        if (isSetDes && !isArrivedDes)
        {
            gameObject.transform.position = Vector3.MoveTowards(transform.position, destination, spaceship.currentSpeed * Time.deltaTime);
            isArrivedDes = destination == gameObject.transform.position;
        }
        else
        {
            gameObject.transform.position += Vector3.left * spaceship.currentSpeed * Time.deltaTime;
        }
    }

    private void OnRoute()
    {
        if (pointIdx >= routePoints.Length)
        {
            return;
        }
        if (gameObject.transform.position != routePoints[pointIdx].position)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, routePoints[pointIdx].position, spaceship.currentSpeed * Time.deltaTime);
        }
        SetDirection((routePoints[pointIdx].position - gameObject.transform.position).normalized);
        if (routePoints[pointIdx].position == gameObject.transform.position)
        {
            pointIdx++;
        }
    }

    // 锯齿形
    private void AsSawtooth(float frequency, bool isNormalized)
    {
        float sinRet = Mathf.Cos(doublePi * appearTime / frequency);
        int revise = 0;
        if (sinRet > 0)
        {
            revise = 1;
        }
        else if (sinRet < 0)
        {
            revise = -1;
        }
        Vector3 direction = Vector3.left + Vector3.up * revise;
        if (isNormalized)
        {
            direction = (Vector3.left + Vector3.up * revise).normalized;
        }
        gameObject.transform.position += direction * spaceship.currentSpeed * Time.deltaTime;
        SetDirection(direction);
    }

    private void AsSine(float frequency, float amplitude, bool isNormalized)
    {
        float revise = Mathf.Cos(doublePi * appearTime / frequency) * amplitude;
        Vector3 direction = Vector3.left + Vector3.up * revise;
        if (isNormalized)
        {
            direction = (Vector3.left + Vector3.up * revise).normalized;
        }
        gameObject.transform.position += direction * spaceship.currentSpeed * Time.deltaTime;
        SetDirection(direction);
    }

    private void AsBillow(float frequency, float amplitude, bool isNormalized)
    {
        float sinRet = Mathf.Sin(doublePi * appearTime / frequency);
        float cosRet = Mathf.Cos(doublePi * appearTime / frequency);
        float revise = cosRet * amplitude;
        if (sinRet < 0)
        {
            revise = -revise;
        }
        Vector3 direction = Vector3.left + Vector3.up * revise;
        if (isNormalized)
        {
            direction = (Vector3.left + Vector3.up * revise).normalized;
        }
        gameObject.transform.position += direction * spaceship.currentSpeed * Time.deltaTime;
        SetDirection(direction);
    }
}

public enum MoveMode
{
    // 有指定路线的
    Route = 1,
    // 锯齿形
    Sawtooth = 2,
    // 正弦形
    Sine = 3,
    // 波浪形
    Billow = 4,
    Z = 5
}
