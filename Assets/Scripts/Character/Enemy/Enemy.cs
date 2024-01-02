
using UnityEngine;

public class Enemy : Character
{
    

    void Update() {

        UpdateByChild();

    }

    public override bool Animate(string animation, string[] animArgs)
    {
        try {
            return base.Animate(animation, animArgs);
        } catch(AnimationCommandException) {
        }

        AnimatorStateInfo currentAnimationState = animator.GetCurrentAnimatorStateInfo(0);
        if(!currentAnimationState.IsName(animation)) {
            switch(animation) {
                default:
                    throw new AnimationCommandException();
            }
        } else {
            return true;
        }
        
    }

}
