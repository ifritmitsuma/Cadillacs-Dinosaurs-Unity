using System;
using UnityEngine;

public class PlayerAnimation : CharacterAnimation {

    public PlayerSoundSelection playerSounds;

    void Start() {
        base.StartByChild();
    }

    public void SoundSpecial() {
        Sound(playerSounds.special);
    }

    public void SoundDie() {
        Sound(playerSounds.die);
    }

    public void SoundDelicious()
    {
        Sound(playerSounds.delicious);
    }

    public void SoundCombo1() {
        Sound(playerSounds.combo1);
    }

    public void SoundCombo2() {
        Sound(playerSounds.combo2);
    }

    public void SoundRunningAttack() {
        Sound(playerSounds.runningAttack);
    }
}