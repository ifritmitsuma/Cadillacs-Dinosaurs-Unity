using UnityEngine;

public class Weapon : Item {

    public int ammoLeft;

    protected override void Start() {
        base.Start();

        ammoLeft = ((WeaponInfo) itemInfo).ammo;
        uiIcon = ((WeaponInfo) itemInfo).icon;

    }

    void OnTriggerStay2D(Collider2D other) {
        OnTriggerStay2DByChild(other);
        other.TryGetComponent(out Player player);
        if(player != null && player.state == CharacterStateEnum.PICKING) {
            player.SetWeapon(this);
        }
    }

}