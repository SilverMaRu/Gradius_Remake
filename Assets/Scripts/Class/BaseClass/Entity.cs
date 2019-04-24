using UnityEngine;

namespace Assets.Scripts.Class.BaseClass
{
    public class Entity : Something
    {
        public int maxHp;
        public int baseHp;
        public int currentHp { get; protected set; }
        [Header("Energy类型穿过时消耗的Energy")]
        public int penetrateImmunity;
        public bool isInvincible { get; protected set; }
        public bool isAlive { get; protected set; }
        
        protected override void InitCurrentAttr()
        {
            currentHp = baseHp;
            isAlive = true;
        }

        protected virtual void Die()
        {
            isAlive = false;
            if (objectPool == null)
            {
                Destroy(gameObject);
            }
            else
            {
                objectPool.Recycling(gameObject);
            }
        }

        public virtual void Hurt(int damage)
        {
            if (damage <= 0 || isInvincible)
            {
                return;
            }
            currentHp -= damage;
            if (currentHp <= 0)
            {
                Die();
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            Entity entity = collision.GetComponent<Entity>();
            if (entity)
            {
                if(team != entity.team)
                {
                    entity.Hurt(atk);
                }
            }

            Energy energy = collision.GetComponent<Energy>();
            if (energy)
            {
                if(team != energy.team)
                {
                    energy.HitSomething(penetrateImmunity);
                }
            }
        }
    }
}
