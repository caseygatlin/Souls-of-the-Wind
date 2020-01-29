using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDust : MonoBehaviour
{
    // Update is called once per frame
    private void FixedUpdate()
    {
        // Management of jump dust -------------------------------------------------------------------------------------
        GameObject dummy = GameObject.Find("DustAnimator(Clone)");
        if (dummy != null)
        {
            Animator dummyAnim = dummy.GetComponent<Animator>();
            AnimatorStateInfo dummyAnimStateInf = dummyAnim.GetCurrentAnimatorStateInfo(0);
            if (dummyAnimStateInf.IsName("Done"))
                Destroy(dummy);
        }

        GameObject dummyDeathObj = GameObject.Find("DeathParticleBurst(Clone)");
        if (dummyDeathObj != null)
        {
            ParticleSystem dummyPS = dummyDeathObj.GetComponent<ParticleSystem>();
            if (!dummyPS.isPlaying)
                Destroy(dummyDeathObj);
        }
    }
}
