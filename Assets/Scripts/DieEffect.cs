using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieEffect : MonoBehaviour
{
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
            Destroy(gameObject);
        }
    }
}
