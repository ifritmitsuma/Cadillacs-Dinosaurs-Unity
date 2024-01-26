using System;
using UnityEngine;

public class Player : Character
{

    public int index;
    float walkToRun;

    float previousMoveX;

    private bool movementAttack = false;

    public PlayerInfo info;

    public Weapon equippedWeapon;

    private int hitCount = 0;

    private bool inTimeCombo = false;

    private PlayerAnimation playerAnimation;

    void Awake() {
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
    }

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
            if(paused) {
                animator.SetFloat("moveMagnitude", 0.0f);
                return;
            }
        }

        UpdateByChild();

        if(inCutscene || state == CharacterStateEnum.DYING) {
            return;
        }

        if(Input.GetButtonDown("Punch")) {
            switch(state) {
                case CharacterStateEnum.RUNNING:
                    RunningAttack();
                    break;
                case CharacterStateEnum.JUMPING:
                    JumpingAttack();
                    break;
                case CharacterStateEnum.WALKING:
                case CharacterStateEnum.IDLE:
                    if(Input.GetButton("Jump")) {
                        Special();
                    } else {
                        Punch();
                    }
                    return;
                default:
                    return;
            }
        }

        // Movement

        previousMoveX = moveX;

        if(!movementAttack) {
            moveX = Input.GetAxisRaw("Horizontal");
            moveY = Input.GetAxisRaw("Vertical");
        }

        if(Input.GetButtonDown("Debug Validate")) {
            GetHit(100);
        }

        VerifyWalkToRun(previousMoveX);

        animator.SetFloat("moveMagnitude", new Vector3(moveX, moveY, 0.0f).magnitude);

    }

    void FixedUpdate() {

        if(moveX != 0.0f || moveY != 0.0f) {
            Move(movementAttack);
        }

    }

    private void VerifyWalkToRun(float previousMoveX) {
        
        if(Input.GetButton("Run") && previousMoveX > 0.0f && !movementAttack) {
            state = CharacterStateEnum.RUNNING;
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
            if(Mathf.Abs(walkToRun) < Mathf.Abs(moveX) && Mathf.Abs(walkToRun) > 0 && !movementAttack) {
                state = CharacterStateEnum.RUNNING;
            } else {
                walkToRun = moveX;
            }
        } else if(previousMoveX != moveX && walkToRun != 0) {
            walkToRun *= -1;
        } else if (moveX == 0 && state == CharacterStateEnum.RUNNING) {
            state = CharacterStateEnum.IDLE;
        }

    }

    public override bool Animate(string animation, string[] animArgs, bool firstTime = true)
    {
        try {
            return base.Animate(animation, animArgs, firstTime);
        } catch(AnimationCommandException) {
        }

        switch(animation) {
            case "punch":
                if(firstTime) {
                    Punch();
                    return false;
                }
                break;
            case "special":
                if(firstTime) {
                    Special();
                    return false;
                }
                break;
            case "pickup":
                if(firstTime) {
                    Pickup();
                    return false;
                }
                break;
            default:
                throw new AnimationCommandException(animation);
        }
        
        return !AnimationManager.GetInstance().IsAnimationPlaying(animator, animation);
        
    }

    private void Punch()
    {
        state = CharacterStateEnum.ATTACKING;
        string animation = "punch";
        if(hitCount == 2) {
            animation = "combo1";
        } else if(hitCount == 3) {
            animation = "combo2";
        }
        AnimationManager.GetInstance().Play(animator, animation, () => {
            if(hitCount == 0) {
                playerAnimation.SoundMiss();
            }
            state = CharacterStateEnum.IDLE;
            if(hitCount > 3) {
                inTimeCombo = false;
                hitCount = 0;
            }
        }, true);
    }

    private void Special()
    {
        if(state != CharacterStateEnum.ATTACKING) {
            state = CharacterStateEnum.ATTACKING;
            AnimationManager.GetInstance().Play(animator, "special", () => {
                state = CharacterStateEnum.IDLE;
            });
        }
    }

    private void Pickup()
    {
        if(state != CharacterStateEnum.PICKING) {
            state = CharacterStateEnum.PICKING;
            AnimationManager.GetInstance().Play(animator, "pickup", () => {
                state = CharacterStateEnum.IDLE;
            });
        }
    }

    private void JumpingAttack()
    {
        throw new NotImplementedException();
    }

    private void RunningAttack()
    {
        if(state != CharacterStateEnum.ATTACKING) {
            movementAttack = true;
            state = CharacterStateEnum.ATTACKING;
            AnimationManager.GetInstance().Play(animator, "runningAttack", () => {
                movementAttack = false;
                state = CharacterStateEnum.IDLE;
            });
        }
    }

    public void GetHit(int damage) {
        GameManager.GetInstance().UpdateHP(this, -damage);
    }

    public void SetWeapon(Weapon weapon)
    {
        this.equippedWeapon = weapon;
    }

    public void HitEnemy() {
        if(inTimeCombo) {
            CancelInvoke(nameof(TimeoutCombo));
        }
        hitCount++;
        inTimeCombo = true;
        Invoke(nameof(TimeoutCombo), 1);
    }

    private void TimeoutCombo() {
        inTimeCombo = false;
        hitCount = 0;
    }

}
