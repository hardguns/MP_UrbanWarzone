using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animManager : MonoBehaviour
{
    public Animation am;
    public AnimationClip walk;
    public AnimationClip idle;

    public bool isTP = false;

    public PhotonView graphicsPV;
    public Animation graphicsAM;
    public AnimationClip tpRun;
    public AnimationClip tpIdle;
    public AnimationClip tpShoot;
    public AnimationClip tpRunShoot;
    public AnimationClip tpReload;

    public Rigidbody rb;

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude >= 0.1)
        {
            if (walk != null)
            {
                playAnim(walk.name);
            }
            graphicsPV.RPC("playAnimPV", PhotonTargets.All, tpRun.name);
        }
        else
        {
            if (!graphicsAM.IsPlaying(tpReload.name))
            {
                if (!isTP)
                {
                    playAnim(idle.name);
                }
                else
                {
                    graphicsPV.RPC("playAnimPV", PhotonTargets.All, tpIdle.name);
                }
            }
        }
    }

    public void playAnim(string animName)
    {
        if (!isTP)
        {
            am.CrossFade(animName, 0.1f);
        }
    }

    public void stopAnim()
    {
        am.Stop();
    }

    public void reload()
    {
        graphicsPV.RPC("playAnimPV", PhotonTargets.All, tpReload.name);
    }

    [PunRPC]
    public void playAnimPV(string animName)
    {
        if (isTP)
        {
            graphicsAM.CrossFade(animName);
        }
    }
}
