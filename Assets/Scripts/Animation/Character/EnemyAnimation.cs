using UnityEditor;

public class EnemyAnimation : CharacterAnimation {

    protected EnemySoundSelection enemySounds;

    void Start() {
        base.StartByChild();
        enemySounds = AssetDatabase.LoadAssetAtPath<EnemySoundSelection>("Assets/Scripts/ScriptableObjects/Sound/EnemySoundSelection.asset");
    }

    protected override void StartByChild()
    {
        Start();
    }

    public virtual void SoundDie() {
        AudioManager.GetInstance().PlaySound(enemySounds.die);
    }
    
}