using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    public GameObject followTarget;
    public bool useTargetSpeed = true;
    public float speed;
    public float followDistance = 3;


    private GameObject barrel;
    private GameObject bullet;

    private Vector3[] track;
    private int trackLength;

    private Collider2D coll;

    // Start is called before the first frame update
    void Start()
    {
        InitCurrentAttr();
        InitGameObjects();
        InitComponents();
    }

    // Update is called once per frame
    void Update()
    {
        Follow();
    }

    public void InitCurrentAttr()
    {
        // 设定移动速度
        if(followTarget != null && useTargetSpeed)
        {
            Spaceship spaceshipScript = followTarget.GetComponent<Spaceship>();
            Option optionScript = followTarget.GetComponent<Option>();
            if(spaceshipScript != null)
            {
                speed = spaceshipScript.currentSpeed;
            }
            else if(optionScript != null)
            {
                speed = optionScript.speed;
            }
        }
        // 计算跟踪数组长度
        trackLength = (int)(followDistance / speed / 0.015);
        // 初始化跟踪数组
        track = new Vector3[trackLength];
        InitTrack();
    }

    private void InitGameObjects()
    {
        FindBarrel();
    }

    private void InitComponents()
    {
        coll = gameObject.GetComponent<Collider2D>();
    }

    private void FindBarrel()
    {
        Transform barrelTransform = transform.Find("Barrel");
        if (barrelTransform != null)
        {
            barrel = barrelTransform.gameObject;
        }
    }

    private void InitTrack()
    {
        for (int i = 0; i < track.Length; i++)
        {
            track[i] = followTarget.transform.position;
        }
    }

    private void Follow()
    {
        if (followTarget == null || track == null)
        {
            return;
        }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
        {
            RememberTrack();
        }
        else
        {
            RefreshTrack();
        }
        Move();
    }

    private void RememberTrack()
    {
        if(followTarget.transform.position != track[0])
        {
            track = Tool.Prepend(followTarget.transform.position, track, false);
        }
    }

    private void RefreshTrack()
    {
        Vector3 deltaVec = followTarget.transform.position - track[0];
        for (int i = 0; i < track.Length; i++)
        {
            track[i] = track[i] + deltaVec;
        }
    }

    private void Move()
    {
        Vector3 limitPoint = track[track.Length - 1];
        if (limitPoint != Vector3.zero)
        {
            transform.position = limitPoint;
        }
    }

    public void SetBullet(GameObject bullet)
    {
        this.bullet = bullet;
    }

    public void Shoot()
    {
        Instantiate(bullet, barrel.transform.position, barrel.transform.rotation);
    }

    public void LostMaster(Transform newParent)
    {
        transform.parent = newParent;
        followTarget = null;
        coll.enabled = true;
    }

    // 被拾取
    public void PickedUp(Transform master, GameObject followTarget)
    {
        coll.enabled = false;
        this.followTarget = followTarget;
        transform.parent = master;
        InitTrack();
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; track != null && i < track.Length; i++)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(track[i], 0.1f);
        }
    }
}
