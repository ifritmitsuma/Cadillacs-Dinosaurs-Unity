using UnityEngine;

public class HitCollisionScript : MonoBehaviour
{

    public Player player;

    public PlayerAnimation playerAnimation;

    public GameObject animationObject;

    public AnimationScriptableObject hitAnimation;
    
    void OnTriggerEnter2D(Collider2D other) {
        if(player.IsAttacking()) {
            playerAnimation.SoundHit();
            if(other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Enemy")) {
                CreateContactAnimation(other.transform);
            }
            if(other.gameObject.CompareTag("Enemy")) {
                player.HitEnemy();
            }
        }
    }

    void CreateContactAnimation(Transform otherTransform) {
        GameObject hitAnimationObject = Instantiate(animationObject, otherTransform);
        hitAnimationObject.transform.position += new Vector3(0.0f, otherTransform.GetComponent<SpriteRenderer>().localBounds.max.y / 2.0f, 0.0f);
        hitAnimationObject.GetComponent<AnimationScript>().anim = hitAnimation; 
    }

}
