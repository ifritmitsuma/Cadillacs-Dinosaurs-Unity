using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    public AnimationScriptableObject anim;

    private Animator animator;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    void Start() {
        animator.runtimeAnimatorController = anim.animatorController;
    }

}
