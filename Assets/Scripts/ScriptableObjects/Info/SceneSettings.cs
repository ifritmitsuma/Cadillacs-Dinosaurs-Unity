using UnityEngine;

[CreateAssetMenu(fileName = "SceneSettings", menuName = "SceneSettings", order = 0)]
public class SceneSettings : ScriptableObject
{

    public string SceneName;
    
    public Vector3 Player1Pos;

    public Vector3 Player2Pos;

}
