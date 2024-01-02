using UnityEditor;
using UnityEngine;

public class EnemyAnimation : CharacterAnimation {

    protected EnemySoundSelection enemySounds;

    void Start() {
        base.StartByChild();
        enemySounds = Resources.Load<EnemySoundSelection>("ScriptableObjects/Sound/EnemySoundSelection");
    }

    protected override void StartByChild()
    {
        Start();
    }

    public virtual void SoundDie() {
        AudioManager.GetInstance().PlaySound(enemySounds.die);
    }
    
}