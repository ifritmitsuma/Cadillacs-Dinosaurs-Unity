using UnityEditor;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour {

    protected Animator animator;

    protected SoundSelection sounds;

    void Start() {
        animator = GetComponent<Animator>();
        sounds = Resources.Load<SoundSelection>("ScriptableObjects/Sound/AllSoundSelection");
    }

    protected virtual void StartByChild() {
        Start();
    }

    public void SoundGunfire() {
        AudioManager.GetInstance().PlaySound(sounds.gunfire);
    }

    
}