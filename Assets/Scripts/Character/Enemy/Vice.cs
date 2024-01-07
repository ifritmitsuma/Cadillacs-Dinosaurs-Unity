public class Vice : Boss
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
            case "shootUp":
                if(firstTime) {
                    ShootUp();
                    return false;
                }
                break;
            default:
                throw new AnimationCommandException();
        }

        return AnimationManager.GetInstance().IsAnimationPlaying(animator, animation);
    }

    public void ShootUp()
    {
        if(!paused)
            AnimationManager.GetInstance().Play(animator, "shootUp");
    }

}
