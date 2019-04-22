using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Box : MonoBehaviour
{
    public BoxType boxType = BoxType.powerBox;

    private const string POWER_BOX_PATH = "Prefabs/Boxs/PowerBox";
    private const string HP_BOX_PATH = "Prefabs/Boxs/RevivalBox";
    private GameObject box;

    // Start is called before the first frame update
    void Start()
    {
        LoadBox();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LoadBox()
    {
        string path = string.Empty;
        BoxTypeSwitch(
            () => { path = POWER_BOX_PATH; }
            , () => { path = HP_BOX_PATH; }
        );
        box = Resources.Load<GameObject>(path);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        VicViper vicViper = collision.GetComponent<VicViper>();
        if(vicViper != null)
        {
            BoxTypeSwitch(
                () => { vicViper.GetPowerBox(); }
                , () => { vicViper.GetRevivalBox(); }
                );
            Destroy(gameObject);
        }
    }

    private void BoxTypeSwitch(Action powerTypeAction, Action hpTypeAction)
    {
        switch (boxType)
        {
            case BoxType.powerBox:
                powerTypeAction();
                break;
            case BoxType.revivalBox:
                hpTypeAction();
                break;
        }
    }
}

public enum BoxType
{
    powerBox = 1,
    revivalBox = 2
}
