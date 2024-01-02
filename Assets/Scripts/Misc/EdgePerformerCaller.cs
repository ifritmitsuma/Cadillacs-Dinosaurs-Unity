using UnityEngine;

public class EdgePerformerCaller : MonoBehaviour
{

    public string act;

    public FollowPlayer followPlayer;

    void OnTriggerEnter2D(Collider2D other) {
        GameManager.GetInstance().MakeHaste(act, followPlayer);
    }

}
