using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Award
{
    public string prefabPath;
    // 出现几率(0,1]
    [Header("出现几率(0,1]")]
    public float odds;

    public Award(string prefabPath, float odds)
    {
        this.prefabPath = prefabPath;
        this.odds = odds;
    }
}

public class Reward : MonoBehaviour
{
    public Award[] awards = new Award[2]{ new Award("Prefabs/Boxs/PowerBox", .3f), new Award("Prefabs/Boxs/RevivalBox", .1f) } ;
    public bool inLine = false;

    private GameObject[] awardGameObjs;
    private Leader leaderScript;
    private float oddsBuildAward = 0f;

    // Start is called before the first frame update
    void Start()
    {
        awardGameObjs = new GameObject[awards.Length];
        for(int i = 0; i < awards.Length; i++)
        {
            awardGameObjs[i] = Resources.Load<GameObject>(awards[i].prefabPath);
        }

        for (int i = 0; i < awards.Length; i++)
        {
            float odds = awards[i].odds;
            while (odds > 1)
            {
                odds = odds * .1f;
            }
            oddsBuildAward += odds;
        }
        if (oddsBuildAward > 1)
        {
            for (int i = 0; i < awards.Length; i++)
            {
                awards[i].odds = awards[i].odds / oddsBuildAward;
            }
        }

        if (inLine)
        {
            leaderScript = transform.parent.gameObject.GetComponent<Leader>();
        }
    }

    //private void OnDestroy()
    //{
    //    if (Application.isPlaying && (leaderScript == null || leaderScript.WillAce()))
    //    {
    //        BuildAward();
    //    }
    //}

    public void BuildAward()
    {
        if (leaderScript == null || leaderScript.WillAce())
        {
            float randomRet = Random.Range(0f, 1f);
            float amout = 0;
            for (int i = 0; i < awards.Length; i++)
            {
                if (amout < randomRet && randomRet <= amout + awards[i].odds)
                {
                    Instantiate(awardGameObjs[i], transform.position, Quaternion.identity);
                    break;
                }
                else
                {
                    amout += awards[i].odds;
                }
            }
        }
    }
}
