using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScenePerformerCommand : OneShotPerformerCommand {
    internal Action unloadFinished;

    public ScenePerformerCommand()
    {
        this.command = PerformerCommandEnum.SCENE;
    }

    public override PerformerTime Execute(Camera camera, Dictionary<string, GameObject> gameObjects, bool ff, bool firstTime = true)
    {

        UIManager.GetInstance().OverlayFade(false, () => {
            SceneLoaderScript.GetInstance().LoadScene(arguments[0], unloadFinished);
            if(arguments.Length == 1 || arguments[1] == "true")
                AudioManager.GetInstance().StopMusic();
        });

        return base.Execute(camera, gameObjects, ff);

    }

}