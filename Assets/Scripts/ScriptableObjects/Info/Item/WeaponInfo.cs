using UnityEngine;

[CreateAssetMenu(fileName = "WeaponInfo", menuName = "Item/WeaponInfo", order = 0)]
public class WeaponInfo : ItemInfo {

    public Sprite icon;

    public WeaponEnum weaponEnum;

    public int ammo;

    public int damage;

}
