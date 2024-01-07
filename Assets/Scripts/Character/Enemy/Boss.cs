
using UnityEngine;

public class Boss : Enemy
{

    void Update() {

        UpdateByChild();

    }

    public override bool Animate(string animation, string[] animArgs, bool firstTime = true)
    {
        try {
            return base.Animate(animation, animArgs, firstTime);
        } catch(AnimationCommandException) {
        }

        switch(animation) {
            case "die":
                if(firstTime) {
                    return false;
                }
                break;
            default:
                throw new AnimationCommandException();
        }

        return !AnimationManager.GetInstance().IsAnimationPlaying(animator, animation);
        
    }

}
