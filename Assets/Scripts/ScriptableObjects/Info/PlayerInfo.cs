using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "PlayerInfo", order = 0)]
public class PlayerInfo : ScriptableObject {

    public Sprite avatar;

    public new string name;

    public RuntimeAnimatorController animator;

}