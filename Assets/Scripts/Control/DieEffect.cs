using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Control
{
    public class DieEffect : MonoBehaviour
    {
        public GameObjectPool.ObjectPool sourcePool { get; set; }

        private Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            animator = gameObject.GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            AutoDestroy();
        }

        private void AutoDestroy()
        {
            AnimatorStateInfo currentAnimatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (currentAnimatorStateInfo.normalizedTime >= 1.0f)
            {
                GameObjectPool.PoolTool.Recycling(gameObject);
            }
        }
    }
}
