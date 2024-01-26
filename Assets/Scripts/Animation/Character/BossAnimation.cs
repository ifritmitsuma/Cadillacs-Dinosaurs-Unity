public class BossAnimation : EnemyAnimation {

    public BossSoundSelection bossSounds;

    void Start() {
        base.StartByChild();
    }

    public override void SoundDie() {
        Sound(bossSounds.die);
    }
    
}