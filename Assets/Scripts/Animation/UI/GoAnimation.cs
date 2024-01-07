using UnityEngine;

public class GoAnimation : MonoBehaviour
{

    public AudioClip goAudioClip;

    public void SoundGo() {
        AudioManager.GetInstance().PlaySound(goAudioClip);
    }


}
