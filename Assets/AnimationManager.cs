using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{

    private static AnimationManager instance;

    private readonly Dictionary<Animator, int> activeAnimations = new();

    public static AnimationManager GetInstance() {
        return instance;
    }

    void Awake() {
        instance = this;
    }

    public void Play(Animator animator, string animation) {
        animator.Play(animation);
        activeAnimations[animator] = Animator.StringToHash("Base Layer." + animation);
    }

    private bool IsAnimationPlaying(Animator animator, int animationHash) {
        return activeAnimations.ContainsKey(animator) && activeAnimations[animator] == animationHash;
    }

    public bool IsAnimationPlaying(Animator animator, string animation) {
        return IsAnimationPlaying(animator, Animator.StringToHash("Base Layer." + animation));
    }

    public void EndAnimation(Animator animator, int animationHash) {
        if(IsAnimationPlaying(animator, animationHash)) {
            activeAnimations.Remove(animator);
        }
    }

    
}
