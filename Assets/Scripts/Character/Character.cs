using System;
using UnityEngine;

public abstract class Character : MonoBehaviour, IPauseListener, ICutsceneListener, IAnimatable
{

    private readonly float moveToDisappear = 10000.0f;

    public float speed = 5.0f;

    protected Animator animator;

    protected Transform animationChild;

    protected float moveX;
    protected float moveY;

    protected bool run;

    protected bool rightDirection = true;

    protected bool paused;

    protected bool inCutscene;

    public bool dead;

    public bool dying;

    public bool attacking;

    private Vector3? relativePosition = null;

    void Start() {
        GameManager.GetInstance().RegisterPauseListener(this);
        PerformerManager.GetInstance().RegisterCutsceneListener(this);

        animationChild = transform.Find("Animation");
        
        animator = animationChild.GetComponent<Animator>();

        rightDirection = animationChild.localScale.x > 0;
    }

    protected virtual void StartByChild() {
        Start();
    }

    void Update() {

        if((rightDirection && animationChild.localScale.x < 0) || (!rightDirection && animationChild.localScale.x > 0)) {
            animationChild.localScale = new Vector3(animationChild.localScale.x * -1, animationChild.localScale.y, animationChild.localScale.z);
        } 

    }

    protected virtual void UpdateByChild() {
        Update();
    }

    public virtual bool Animate(string animation, string[] animArgs = null)
    {

        if(animator == null) {
            return true;
        }

        AnimatorStateInfo currentAnimationState = animator.GetCurrentAnimatorStateInfo(0);
        if(!currentAnimationState.IsName(animation)) {
            switch(animation) {
                case "walk":
                    return Move(animArgs, false);
                case "run":
                    return Move(animArgs, true);
                case "die":
                    if(!dead && !dying) {
                        GameManager.GetInstance().KillCharacter(this);
                    }
                    return dead;
                default:
                    throw new AnimationCommandException();
            }
        } else {
            return true;
        }
    }

    public bool Move(string[] args, bool run) {

        if(args.Length == 0) {
            return true;
        }

        float absoluteX = 0, absoluteY = 0;
        string relative = null;
        try
        {
            absoluteX = float.Parse(args[0]);
            absoluteY = float.Parse(args[1]);
        } catch (FormatException)
        {
            switch(args[0]) {
                case "right":
                    absoluteX = moveToDisappear;
                    absoluteY = transform.position.y;
                    rightDirection = true;
                    break;
                case "left":
                    absoluteX = -moveToDisappear;
                    absoluteY = transform.position.y;
                    rightDirection = false;
                    break;
                default:
                    relative = args[0];
                    break;
            }
        }

        if(relative != null) {
            return MoveTowards(GameObject.Find(relative), run);
        } else {
            return MoveTowards(new Vector3(absoluteX, absoluteY, transform.position.z), run);
        }

    }

    public bool MoveTowards(Vector3 otherPosition, bool run)
    {
        Vector3 diff = new(transform.position.x > otherPosition.x ? -1.5f : 1.5f, transform.position.y > otherPosition.y ? -0.3f : 0.3f, 0);
        
        if(transform.position == otherPosition - diff || (Mathf.Abs(otherPosition.x) == moveToDisappear && IsOffScreen())) {
            relativePosition = null;
            return true;
        }
        relativePosition = otherPosition;
        animator.Play(run ? "run" : "walk");
        animator.SetFloat("moveMagnitude", 1.0f);
        return false;
    }

    public bool MoveTowards(GameObject other, bool run) {
        return MoveTowards(other.transform.position, run);
    }

    private void LateUpdate()
    {

        if (relativePosition == null)
        {
            return;
        }

        Vector3 relPosition = relativePosition.Value;

        Vector3 diff = new(transform.position.x > relPosition.x ? -1.5f : 1.5f, transform.position.y > relPosition.y ? -0.3f : 0.3f, 0);
        if (transform.position == relPosition - diff)
        {
            animator.SetFloat("moveMagnitude", 0.0f);
            relativePosition = null;
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, relPosition - diff, (run ? 2.0f : 1.0f) * speed * Time.deltaTime);

        if (Mathf.Abs(relPosition.x) == moveToDisappear && IsOffScreen())
        {
            animator.SetFloat("moveMagnitude", 0.0f);
            relativePosition = null;
            if(this is not Player) {
                DestroyImmediate(gameObject);
            }
        }

    }

    private bool IsOffScreen()
    {
        return transform.position.x > Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect * 1.5f;
    }

    
    protected void MoveY()
    {
        if(run) {
            animator.Play("run");
        } else {
            animator.Play("walk");
        }
        transform.position += new Vector3(0.0f, (moveY > 0.0f ? 0.01f : -0.01f) * (run ? 2.0f : 1.0f) * speed / 2.0f, 0.0f);
    }

    protected void MoveX()
    {

        rightDirection = moveX > 0.0f;

        if(run) {
            animator.Play("run");
        } else {
            animator.Play("walk");
        }
        transform.position += new Vector3((rightDirection ? 0.01f : -0.01f) * (run ? 2.0f : 1.0f) * speed, 0.0f, 0.0f);
    }

    public bool Die() {
        if(!dead && !dying) {
            animator.Play("die");
            dying = true;
        }
        return dead;        
    }

    public virtual void Pause()
    {
        paused = true;
        if(animator) {
            animator.speed = 0.0f;
        }
    }

    public virtual void Unpause()
    {
        paused = false;
        if(animator) {
            animator.speed = 1.0f;
        }
    }

    public void CutsceneStarted()
    {
        inCutscene = true;
    }

    public void CutsceneEnded()
    {
        inCutscene = false;
    }

    public void FastForward(bool ff)
    {
        if(animator)
            animator.speed = ff ? 3.0f : 1.0f;
        speed = ff ? speed * 3.0f : speed / 3.0f;
    }

    public bool IsInAnimation(string animation) {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animation);
    }
}