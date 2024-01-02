
using UnityEngine;

public class Boss : Enemy
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
                case "die":
                    return false;
                default:
                    throw new AnimationCommandException();
            }
        } else {
            return true;
        }
    }

}
