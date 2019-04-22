using UnityEngine;

public interface IShoot
{
    Assets.Scripts.Class.Weapons.Weapon[] bullets { get; set; }
    // 射击频率
    float shootFrequency { get; set; }
    Vector3 shootDirection { get; set; }
    GameObject barrel { get; set; }
    int bulletIdx { get; set; }

    void Shoot();
}
