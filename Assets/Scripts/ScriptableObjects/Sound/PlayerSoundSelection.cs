using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSoundSelection", menuName = "SoundSelection/Player", order = 1)]
public class PlayerSoundSelection : ScriptableObject {

    public AudioClip special;

    public AudioClip die;

    public AudioClip delicious;

    public AudioClip combo1;

    public AudioClip combo2;

    public AudioClip runningAttack;

}