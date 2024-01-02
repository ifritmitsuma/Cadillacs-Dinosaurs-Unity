using UnityEngine;

public class Vice : Boss
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
                case "shootUp":
                    ShootUp();
                    return false;
                default:
                    throw new AnimationCommandException();
            }
        } else {
            return true;
        }
    }

    public void ShootUp()
    {
        if(!paused)
            animator.Play("shootUp");
    }

}
