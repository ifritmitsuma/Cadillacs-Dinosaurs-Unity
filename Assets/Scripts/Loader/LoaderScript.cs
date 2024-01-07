using System;
using System.Collections;
using UnityEngine;

public class LoaderScript : MonoBehaviour
{

    public Transform backgrounds;

    public GameObject furthestBackground;

    public PerformerScript performer;

    public SceneSettings sceneSettings;

    void Start() {
        StartCoroutine(Load());
    }

    IEnumerator Load() {
        
        yield return new WaitUntil(() => {
            return gameObject.scene.isLoaded;
        });
        //Camera.main.transform.localPosition = new Vector3(backgrounds.localPosition.x, backgrounds.localPosition.y, Camera.main.transform.localPosition.z);
        Camera.main.GetComponent<Parallax>().backgrounds = backgrounds;
        Camera.main.GetComponent<FollowPlayer>().backgroundDelimiter = furthestBackground;

        if(sceneSettings == null) {
            throw new SystemException();
        }

        PerformerManager.GetInstance().SetPerformer(performer);

        GameManager.GetInstance().LoadPlayers(sceneSettings.Player1Pos, sceneSettings.Player2Pos, PerformerManager.GetInstance().HasPerformance("Start"));

        if(!string.IsNullOrEmpty(sceneSettings.music)) {
            AudioManager.GetInstance().PlayMusic(sceneSettings.music, true);
        } 

    }

}
