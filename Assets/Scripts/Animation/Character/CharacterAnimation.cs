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
        Sound(sounds.gunfire);
    }

    public void SoundHit() {
        Sound(sounds.hit);
    }

    public void SoundMiss()
    {
        Sound(sounds.miss);
    }

    protected void Sound(AudioClip clip) {
        AudioManager.GetInstance().PlaySound(clip);
    }

    
}