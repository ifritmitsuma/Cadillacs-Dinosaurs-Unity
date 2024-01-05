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

        if(!AnimationManager.GetInstance().IsAnimationPlaying(animator, animation)) {
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
            AnimationManager.GetInstance().Play(animator, "shootUp");
    }

}
