
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

        if(!AnimationManager.GetInstance().IsAnimationPlaying(animator, animation)) {
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
