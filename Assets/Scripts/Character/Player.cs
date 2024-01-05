using UnityEngine;

public class Player : Character
{

    public int index;
    float walkToRun;

    float previousMoveX;
    
    bool runButtonPressed;

    public PlayerInfo info;

    void Start() {
        base.StartByChild();
        Camera.main.GetComponent<FollowPlayer>().player = transform;
        if(info.animator && animator) {
            animator.runtimeAnimatorController = info.animator;
        }
    }

    void Update() {
        
        if(paused || inCutscene) {
            moveX = 0.0f;
            moveY = 0.0f;
            animator.SetFloat("moveMagnitude", 0.0f);
            if(paused) {
                return;
            }
        }

        UpdateByChild();

        if(inCutscene) {
            return;
        }

        if(Input.GetButtonDown("Punch")) {
            attacking = true;
            Punch();
            return;
        }

        attacking = false;

        // Movement

        previousMoveX = moveX;

        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");

        if(Input.GetButtonDown("Debug Validate")) {
            GetHit(100);
        }

        VerifyWalkToRun(previousMoveX);

        animator.SetFloat("moveMagnitude", new Vector3(moveX, moveY, 0.0f).magnitude);

    }

    void FixedUpdate() {

        if(moveX != 0.0f) {
            MoveX();
        }
        
        if(moveY != 0.0f) {
            MoveY();
        }

    }

    private void VerifyWalkToRun(float previousMoveX) {
        
        if(Input.GetButton("Run") || runButtonPressed) {
            run = Input.GetButton("Run");
            runButtonPressed = run;
            return;
        }

        if(walkToRun != 0.0f) {
            float previousWalkToRun = walkToRun;
            walkToRun -= (walkToRun > 0 ? 1.0f : -1.0f) * Time.deltaTime;
            if(previousWalkToRun / walkToRun < 0.0f) {
                walkToRun = 0.0f;
            }
        }
        if(moveX != 0 && previousMoveX == 0) {
            if(Mathf.Abs(walkToRun) < Mathf.Abs(moveX) && Mathf.Abs(walkToRun) > 0) {
                run = true;
            } else {
                walkToRun = moveX;
            }
        } else if(previousMoveX != moveX && walkToRun != 0) {
            walkToRun *= -1;
        } else if (moveX == 0) {
            run = false;
        }

    }

    public override bool Animate(string animation, string[] animArgs)
    {
        try {
            return base.Animate(animation, animArgs);
        } catch(AnimationCommandException) {
        }

        if(!AnimationManager.GetInstance().IsAnimationPlaying(animator, animation)) {
            switch(animation) {
                case "punch":
                    Punch();
                    return false;
                case "special":
                    Special();
                    return false;
                default:
                    throw new AnimationCommandException();
            }
        } else {
            return true;
        }
        
    }

    public void Punch()
    {
        AnimationManager.GetInstance().Play(animator, "punch");
    }

    public void Special()
    {
        AnimationManager.GetInstance().Play(animator, "special");
    }

    public void GetHit(int damage) {
        GameManager.GetInstance().UpdateHP(this, -damage);
    }
}
