using UnityEngine;

public class Obstacle : MonoBehaviour, IAnimatable
{

    public Sprite replacementSprite;

    private SpriteRenderer spriteRenderer;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _ = TryGetComponent<Animator>(out animator);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(spriteRenderer.sprite == replacementSprite) {
            return;
        }

        if(other.gameObject.layer == LayerMask.NameToLayer("Player") && other is CircleCollider2D && other.isTrigger) {
            Player player = other.transform.parent.GetComponent<Player>();
            if(player.attacking) {
                Animate("break");
                GameManager.GetInstance().UpdateScore(player, 100);
            }
        }
    }

    public bool Animate(string animation, string[] animArgs = null)
    {
        if(animator == null || spriteRenderer.sprite == replacementSprite) {
            return true;
        }

        if(!AnimationManager.GetInstance().IsAnimationPlaying(animator, animation)) {
            switch(animation) {
                case "break":
                    AnimationManager.GetInstance().Play(animator, "break");
                    break;
                default:
                    throw new AnimationCommandException();
            }
        } else {
            spriteRenderer.sprite = replacementSprite;
            return true;
        }
        return false;
    }
}
