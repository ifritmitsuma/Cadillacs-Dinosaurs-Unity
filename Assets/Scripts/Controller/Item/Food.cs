using UnityEngine;

public class Food : Item {
    
    void OnTriggerStay2D(Collider2D other) {
        OnTriggerStay2DByChild(other);
        other.TryGetComponent(out Player player);
        if(player != null && player.state == CharacterStateEnum.PICKING) {
            GameManager.GetInstance().UpdateHP(player, ((FoodInfo) itemInfo).hp);
            player.GetComponentInChildren<PlayerAnimation>().SoundDelicious();
        }
    }

}