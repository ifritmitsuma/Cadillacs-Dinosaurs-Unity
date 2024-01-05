using UnityEngine;

public class FollowPlayer : MonoBehaviour, ICutsceneListener
{

    public Transform player;

    public GameObject backgroundDelimiter;
    public bool locked;

    public bool inCutscene;

    private EdgeCollider2D frameCollider;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        PerformerManager.GetInstance().RegisterCutsceneListener(this);
        frameCollider = GetComponent<EdgeCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        frameCollider.enabled = !inCutscene;
        if(!locked && !inCutscene && player != null && backgroundDelimiter != null) {
            if(transform.position.x < player.position.x) {
                rb.MovePosition(new Vector2(player.position.x, player.position.y));
            } else {
                GameManager.GetInstance().MakeHaste();
            }
        } else {
            GameManager.GetInstance().ClearTimer();
        }
    }

    public void CutsceneEnded()
    {
        inCutscene = false;
    }

    public void CutsceneStarted()
    {
        inCutscene = true;
    }

    public void FastForward(bool ff) {
        // Do nothing;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        other.TryGetComponent(out EdgePerformerCaller component);
        if(component) {
            component.followPlayer = this;
        }
        locked = true;
    }

}
