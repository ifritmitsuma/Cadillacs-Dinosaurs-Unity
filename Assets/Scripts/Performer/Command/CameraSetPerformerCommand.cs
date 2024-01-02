using System.Collections.Generic;
using UnityEngine;

public class CameraSetPerformerCommand : OneShotPerformerCommand {

    public CameraSetPerformerCommand()
    {
        this.command = PerformerCommandEnum.CAMS;
    }

    public override PerformerTime Execute(Camera camera, Dictionary<string, GameObject> gameObjects, bool ff)
    {

        camera.transform.position = new Vector3(float.Parse(arguments[0]), float.Parse(arguments[1]), camera.transform.position.z);

        return base.Execute(camera, gameObjects, ff);


    }

}