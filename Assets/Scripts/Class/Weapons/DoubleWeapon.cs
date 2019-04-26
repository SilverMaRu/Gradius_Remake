using UnityEngine;
using Assets.Scripts.GameObjectPool;

namespace Assets.Scripts.Class.Weapons
{
    //[System.Serializable]
    //public class DoubleWeapon: Weapon
    //{
    //    public GameObject secondaryBarrelGameObj;

    //    public DoubleWeapon(GameObject barrelGameObj, float shootFrequency, BaseClass.Team team, BulletInfo[] bulletInfos)
    //        : base(barrelGameObj, shootFrequency, team, bulletInfos)
    //    {
    //        secondaryBarrelGameObj = barrelGameObj;
    //    }

    //    public DoubleWeapon(GameObject barrelGameObj, GameObject secondaryBarrelGameObj, float shootFrequency, BaseClass.Team team, BulletInfo[] bulletInfos) 
    //        : base(barrelGameObj, shootFrequency, team, bulletInfos)
    //    {
    //        this.secondaryBarrelGameObj = secondaryBarrelGameObj;
    //    }

    //    protected override void Shoot()
    //    {
    //        base.Shoot();
    //        GameObject instance = PoolTool.GetGameObject(bulletInfos[usingBulletIdx].poolName, secondaryBarrelGameObj.transform.position, secondaryBarrelGameObj.transform.rotation);
    //        BaseClass.Something something = instance.GetComponent<BaseClass.Something>();
    //        something.team = team;
    //    }
    //}
}
