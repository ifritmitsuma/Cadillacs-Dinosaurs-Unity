using System;
using System.Collections.Generic;
using UnityEngine;

public class ScenePerformerCommand : OneShotPerformerCommand {
    internal Action unloadFinished;

    public ScenePerformerCommand()
    {
        this.command = PerformerCommandEnum.SCENE;
    }

    public override PerformerTime Execute(Camera camera, Dictionary<string, GameObject> gameObjects, bool ff)
    {

        SceneLoaderScript.GetInstance().LoadScene(arguments[0], unloadFinished);

        return base.Execute(camera, gameObjects, ff);

    }

}