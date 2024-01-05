using UnityEngine;

public class EdgePerformerCaller : MonoBehaviour
{

    public string act;

    public FollowPlayer followPlayer;

    void OnTriggerEnter2D(Collider2D other) {
        if(!string.IsNullOrEmpty(act)) {
            GameManager.GetInstance().MakeHaste(act, followPlayer);
        }
    }

}
