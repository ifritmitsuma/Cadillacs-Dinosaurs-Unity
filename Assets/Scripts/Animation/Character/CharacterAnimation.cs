using UnityEditor;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour {

    protected Character character;

    protected Animator animator;

    protected SoundSelection sounds;

    void Start() {
        character = transform.parent.GetComponent<Character>();
        animator = GetComponent<Animator>();
        sounds = AssetDatabase.LoadAssetAtPath<SoundSelection>("Assets/Scripts/ScriptableObjects/Sound/AllSoundSelection.asset");
    }

    protected virtual void StartByChild() {
        Start();
    }

    public void SoundGunfire() {
        AudioManager.GetInstance().PlaySound(sounds.gunfire);
    }

    public void DieAnimationEnded() {
        character.dead = true;
        character.dying = false;
        animator.SetBool("dead", true);
    }

    
}