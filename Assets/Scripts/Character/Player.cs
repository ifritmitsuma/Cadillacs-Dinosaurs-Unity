using System;
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

        UpdateByChild();

        if(Input.GetButtonDown("Punch")) {
            attacking = true;
            animator.Play("punch");
            return;
        }

        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("idle") &&
           !animator.GetCurrentAnimatorStateInfo(0).IsName("walk") &&
           !animator.GetCurrentAnimatorStateInfo(0).IsName("run")) {
            return;
        }

        attacking = false;

        // Movement

        if(paused || inCutscene) {
            moveX = 0.0f;
            moveY = 0.0f;
            return;
        }

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

        AnimatorStateInfo currentAnimationState = animator.GetCurrentAnimatorStateInfo(0);
        if(!currentAnimationState.IsName(animation)) {
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
        animator.Play("punch");
    }

    public void Special()
    {
        animator.Play("special");
    }

    public void GetHit(int damage) {
        GameManager.GetInstance().UpdateHP(this, -damage);
    }
}
