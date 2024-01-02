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
    void Update()
    {
        frameCollider.enabled = !inCutscene;
        if(!locked && !inCutscene && player != null && backgroundDelimiter != null) {
            float bgBoxWidth = backgroundDelimiter.GetComponent<SpriteRenderer>().bounds.size.x;
            if(!(player.position.x < backgroundDelimiter.transform.position.x - bgBoxWidth / 2.0f + GetComponent<Camera>().orthographicSize * GetComponent<Camera>().aspect ||
                 player.position.x > backgroundDelimiter.transform.position.x + bgBoxWidth / 2.0f - GetComponent<Camera>().orthographicSize * GetComponent<Camera>().aspect) &&
                 transform.position.x < player.position.x) {
                
                rb.MovePosition(new Vector2(player.position.x, player.position.y));
                GameManager.GetInstance().MakeHaste();

            }
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
        Destroy(other);
        other.TryGetComponent(out EdgePerformerCaller component);
        if(component) {
            component.followPlayer = this;
        }
        locked = true;
    }

}
