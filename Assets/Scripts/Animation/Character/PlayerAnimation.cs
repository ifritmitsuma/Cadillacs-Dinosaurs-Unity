using UnityEngine;

public class PlayerAnimation : CharacterAnimation {

    public PlayerSoundSelection playerSounds;

    public Player player;

    void Start() {
        base.StartByChild();
    }

    public void SoundSpecial() {
        AudioManager.GetInstance().PlaySound(playerSounds.special);
    }

    public void SoundDie() {
        AudioManager.GetInstance().PlaySound(playerSounds.die);
    }
    
}