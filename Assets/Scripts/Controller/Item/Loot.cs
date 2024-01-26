using UnityEngine;

public class Loot : Item {
    
    void OnTriggerStay2D(Collider2D other) {
        OnTriggerStay2DByChild(other);
        other.TryGetComponent(out Player player);
        if(player != null && player.state == CharacterStateEnum.PICKING) {
            GameManager.GetInstance().UpdateScore(player, ((LootInfo) itemInfo).score);
        }
    }
    
}