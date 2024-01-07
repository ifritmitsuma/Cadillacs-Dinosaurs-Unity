using System;
using System.Globalization;
using UnityEngine;

public abstract class Character : MonoBehaviour, IPauseListener, ICutsceneListener, IAnimatable
{

    private readonly float moveToDisappear = 10000.0f;

    public float speed = 5.0f;

    public Animator animator;

    private Transform animationChild;

    private SpriteRenderer animationSpriteRenderer;

    private Rigidbody2D rb;

    protected float moveX;
    protected float moveY;

    protected bool run;

    protected bool rightDirection = true;

    protected bool paused;

    protected bool inCutscene;

    public bool attacking;

    void Start() {
        GameManager.GetInstance().RegisterPauseListener(this);
        PerformerManager.GetInstance().RegisterCutsceneListener(this);
        
        animationChild = transform.Find("Animation");
        animator = animationChild.GetComponent<Animator>();
        animationSpriteRenderer = animationChild.GetComponent<SpriteRenderer>();

        animationChild.TryGetComponent(out rb);

        rightDirection = transform.localScale.x > 0;
    }

    protected virtual void StartByChild() {
        Start();
    }

    void Update() {

        if((rightDirection && transform.localScale.x < 0) || (!rightDirection && transform.localScale.x > 0)) {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            rightDirection = transform.localScale.x > 0;
        }

        animationSpriteRenderer.sortingOrder = (int) (transform.position.y  * -1000);

    }

    protected virtual void UpdateByChild() {
        Update();
    }

    public virtual bool Animate(string animation, string[] animArgs = null, bool firstTime = true)
    {

        if(animator == null) {
            return true;
        }

        switch(animation) {
            case "walk":
                return Move(animArgs, false);
            case "run":
                return Move(animArgs, true);
            case "die":
                if(firstTime) {
                    GameManager.GetInstance().KillCharacter(this);
                    return false;
                }
                break;
            default:
                throw new AnimationCommandException();
        }

        return !AnimationManager.GetInstance().IsAnimationPlaying(animator, animation);
    }

    public bool Move(string[] args, bool run) {

        if(args.Length == 0) {
            return true;
        }

        float absoluteX = 0, absoluteY = 0;
        string relative = null;
        try
        {
            absoluteX = float.Parse(args[0], CultureInfo.InvariantCulture);
            absoluteY = float.Parse(args[1], CultureInfo.InvariantCulture);
        } catch (FormatException)
        {
            switch(args[0]) {
                case "right":
                    absoluteX = moveToDisappear;
                    absoluteY = transform.position.y;
                    break;
                case "left":
                    absoluteX = -moveToDisappear;
                    absoluteY = transform.position.y;
                    break;
                default:
                    relative = args[0];
                    break;
            }
        }

        if(relative != null) {
            return MoveTowards(GameObject.Find(relative), run, args.Length > 1 && args[1] == "close");
        } else {
            return MoveTowards(new Vector3(absoluteX, absoluteY, transform.position.z), run);
        }

    }

    public bool MoveTowards(Vector3 otherPosition, bool run, bool closeToPosition = false)
    {
        if(closeToPosition) {
            Vector3 diff = new(transform.position.x > otherPosition.x ? -1.5f : 1.5f, transform.position.y > otherPosition.y ? -0.3f : 0.3f, 0);
            otherPosition -= diff;
        }
        
        if (transform.position == otherPosition)
        {
            animator.SetFloat("moveMagnitude", 0.0f);
            return true;
        }

        rightDirection = otherPosition.x > transform.position.x;

        if(rb != null && rb.bodyType != RigidbodyType2D.Static) {
            rb.MovePosition((run ? 2.0f : 1.0f) * speed * Time.deltaTime * otherPosition);
        }
        transform.position = Vector3.MoveTowards(transform.position, otherPosition, (run ? 2.0f : 1.0f) * speed * Time.deltaTime);

        if (Mathf.Abs(otherPosition.x) == moveToDisappear && IsOffScreen())
        {
            animator.SetFloat("moveMagnitude", 0.0f);
            if(this is not Player) {
                DestroyImmediate(gameObject);
            }
            return true;
        }
        
        AnimationManager.GetInstance().Play(animator, run ? "run" : "walk");
        animator.SetFloat("moveMagnitude", 1.0f);
        return false;
    }

    public bool MoveTowards(GameObject other, bool run, bool closeToPosition) {
        return MoveTowards(other.transform.position, run, closeToPosition);
    }

    private bool IsOffScreen()
    {
        return transform.position.x > Camera.main.transform.position.x + Camera.main.orthographicSize * Camera.main.aspect * 1.5f;
    }

    
    protected void MoveY()
    {
        if(run) {
            AnimationManager.GetInstance().Play(animator, "run");
        } else {
            AnimationManager.GetInstance().Play(animator, "walk");
        }
        if(rb != null && rb.bodyType != RigidbodyType2D.Static) {
            rb.MovePosition(new Vector3(0.0f, (moveY > 0.0f ? 0.01f : -0.01f) * (run ? 2.0f : 1.0f) * speed / 2.0f, 0.0f));
        }
        transform.position += new Vector3(0.0f, (moveY > 0.0f ? 0.01f : -0.01f) * (run ? 2.0f : 1.0f) * speed / 2.0f, 0.0f);
    }

    protected void MoveX()
    {

        rightDirection = moveX > 0.0f;

        if(run) {
            AnimationManager.GetInstance().Play(animator, "run");
        } else {
            AnimationManager.GetInstance().Play(animator, "walk");
        }
        if(rb != null && rb.bodyType != RigidbodyType2D.Static) {
            rb.MovePosition(new Vector3((rightDirection ? 0.01f : -0.01f) * (run ? 2.0f : 1.0f) * speed, 0.0f, 0.0f));
        }
        transform.position += new Vector3((rightDirection ? 0.01f : -0.01f) * (run ? 2.0f : 1.0f) * speed, 0.0f, 0.0f);
    }

    public bool Die() {
        AnimationManager.GetInstance().Play(animator, "die");
        return !AnimationManager.GetInstance().IsAnimationPlaying(animator, "die");        
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
}