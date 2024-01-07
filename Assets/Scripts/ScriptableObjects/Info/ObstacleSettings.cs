using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleSettings", menuName = "ObstacleSettings", order = 0)]
public class ObstacleSettings : ScriptableObject {

    public int score;
    public AudioClip breakSound;

    public bool hasPrize;

}