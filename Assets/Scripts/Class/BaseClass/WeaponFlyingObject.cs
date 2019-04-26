
namespace Assets.Scripts.Class.BaseClass
{
    public class WeaponFlyingObject : FlyingObject
    {
        public Weapons.WeaponInfo[] weaponInfos;
        public int currentWeaponIdx { get; protected set; }

        public Weapons.Weapon[] weapons { get; protected set; }

        protected override void Init()
        {
            base.Init();
            InitWeapon();
        }

        protected override void InitCurrentAttr()
        {
            base.InitCurrentAttr();
            currentWeaponIdx = 0;
        }

        protected virtual void InitWeapon()
        {
            weapons = new Weapons.Weapon[weaponInfos.Length];
            for (int i = 0; i < weaponInfos.Length; i++)
            {
                weapons[i] = new Weapons.Weapon(weaponInfos[i].barrelGameObjs, weaponInfos[i].shootFrequency, team, weaponInfos[i].poolName);
            }
        }

        protected virtual void Shoot()
        {
            weapons[currentWeaponIdx].TryShoot();
        }
    }
}
