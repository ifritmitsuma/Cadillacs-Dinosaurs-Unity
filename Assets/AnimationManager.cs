using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{

    private static AnimationManager instance;

    private readonly Dictionary<Animator, int> activeAnimations = new();

    private readonly Dictionary<Animator, Action> activeAnimationCallbacks = new();

    public static AnimationManager GetInstance() {
        return instance;
    }

    void Awake() {
        instance = this;
    }

    public void Play(Animator animator, string animation, Action callback = null) {
        if(!IsAnimationPlaying(animator, animation)) {
            animator.Play(animation);
        }
        activeAnimations[animator] = Animator.StringToHash("Base Layer." + animation);
        if(callback != null) {
            activeAnimationCallbacks[animator] = callback;
        }
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
        if(activeAnimationCallbacks.ContainsKey(animator)) {
            activeAnimationCallbacks[animator].Invoke();
            activeAnimationCallbacks.Remove(animator);
        }
    }

    
}
