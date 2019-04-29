using UnityEngine;

namespace Assets.Scripts.Class.BaseClass
{
    public class Energy : Something
    {
        public int maxEnergy;
        public int baseEnergy;
        public int currentEnergy { get; protected set; }

        protected override void InitCurrentAttr()
        {
            currentEnergy = baseEnergy;
        }

        protected override bool ShouldDisappear()
        {
            return currentEnergy <= 0;
        }

        public virtual void HitSomething(int expend)
        {
            currentEnergy -= expend;
            if (ShouldDisappear())
            {
                Disappear();
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
                    energy.HitSomething(currentEnergy);
                }
            }
        }
    }
}
