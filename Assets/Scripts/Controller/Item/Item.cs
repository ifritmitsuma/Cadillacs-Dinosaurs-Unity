using UnityEngine;

public abstract class Item : MonoBehaviour {
    
    public ItemInfo itemInfo;

    public Sprite uiIcon;

    protected virtual void Start() {
        GetComponentInChildren<SpriteRenderer>().sprite = itemInfo.sprite;
    }

    void OnTriggerStay2D(Collider2D other) {

        other.TryGetComponent(out Player player);
        if(player != null && player.state == CharacterStateEnum.ATTACKING) {
            player.state = CharacterStateEnum.PICKING;
            player.Animate("pickup", new string[] {GetType().Name});
            UIManager.GetInstance().NewPickup(player.index, this);
            Destroy(gameObject);
        }

    }

    protected void OnTriggerStay2DByChild(Collider2D other) {

        OnTriggerStay2D(other);

    }

}