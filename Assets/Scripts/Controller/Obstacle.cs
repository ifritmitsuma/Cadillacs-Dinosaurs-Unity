using UnityEngine;
using UnityEngine.SceneManagement;

public class Obstacle : MonoBehaviour, IAnimatable
{

    private Animator animator;

    public ObstacleSettings obstacleSettings;

    public GameObject prizePrefab;

    // Start is called before the first frame update
    void Start()
    {
        animator = transform.Find("Animation").GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other) {

        if(other.gameObject.layer == LayerMask.NameToLayer("Player") && other is CircleCollider2D && other.isTrigger) {
            Player player = other.GetComponent<HitCollisionScript>().player;
            if(player.IsAttacking()) {
                GetComponent<Collider2D>().enabled = false;
                if(obstacleSettings.breakSound) {
                    AudioManager.GetInstance().PlaySound(obstacleSettings.breakSound);
                }
                AnimationManager.GetInstance().Play(animator, "break", () => {
                    Destroy(gameObject);
                });
                foreach(Collider2D collider in GetComponents<Collider2D>()) {
                    collider.enabled = false;
                }
                GameManager.GetInstance().UpdateScore(player, obstacleSettings.score);
                if(obstacleSettings.hasPrize && prizePrefab) {
                    GameObject prize = Instantiate(prizePrefab, transform.position, transform.rotation);
                    SceneManager.MoveGameObjectToScene(prize, gameObject.scene);
                }
            }
        }
    }

    public bool Animate(string animation, string[] animArgs = null, bool firstTime = true)
    {
        if(animator == null) {
            return true;
        }

        switch(animation) {
            case "break":
                if(firstTime) {
                    if(obstacleSettings.breakSound) {
                        AudioManager.GetInstance().PlaySound(obstacleSettings.breakSound);
                    }
                    AnimationManager.GetInstance().Play(animator, "break");
                    return false;
                }
                break;
            default:
                throw new AnimationCommandException();
        }
        
        return !AnimationManager.GetInstance().IsAnimationPlaying(animator, animation);
    }
}
