using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twinkling : MonoBehaviour
{
    public Sprite normalSprite;
    public GameObject vicViperGameObj;
    public int triggerPowerNum = 1;

    private VicViper vicViper;
    private SpriteRenderer spriteR;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        InitComponent();
        StopTwinkling();
    }

    private void InitComponent()
    {
        vicViper = vicViperGameObj.GetComponent<VicViper>();
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(vicViper != null && vicViper.currentPower == triggerPowerNum)
        {
            StartTwinkling();
        }
        else
        {
            StopTwinkling();
        }
    }

    public void StopTwinkling()
    {
        animator.enabled = false;
        spriteR.sprite = normalSprite;
    }

    public void StartTwinkling()
    {
        animator.enabled = true;
    }
}
