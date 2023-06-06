using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonQueuedTrigger : StateMachineBehaviour
{
    public string triggerParam;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(triggerParam);
    }
}
