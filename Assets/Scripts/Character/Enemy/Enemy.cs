
using UnityEngine;

public class Enemy : Character
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
            default:
                throw new AnimationCommandException();
        }

        return !AnimationManager.GetInstance().IsAnimationPlaying(animator, animation);
        
    }

}
