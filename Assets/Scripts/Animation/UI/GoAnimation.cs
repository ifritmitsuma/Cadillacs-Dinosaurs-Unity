using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoAnimation : MonoBehaviour
{

    public AudioClip goAudioClip;

    public void SoundGo() {
        AudioManager.GetInstance().PlaySound(goAudioClip);
    }


}
