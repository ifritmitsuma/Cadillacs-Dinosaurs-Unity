using System;
using UnityEngine;

public class EndStateMachineBehaviour : StateMachineBehaviour {

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AnimationManager.GetInstance().EndAnimation(animator, stateInfo.fullPathHash);
    }
}