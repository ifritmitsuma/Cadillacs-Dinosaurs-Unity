using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CameraSetPerformerCommand : OneShotPerformerCommand {

    public CameraSetPerformerCommand()
    {
        this.command = PerformerCommandEnum.CAMS;
    }

    public override PerformerTime Execute(Camera camera, Dictionary<string, GameObject> gameObjects, bool ff, bool firstTime = true)
    {

        camera.transform.position = new Vector3(float.Parse(arguments[0], CultureInfo.InvariantCulture), float.Parse(arguments[1], CultureInfo.InvariantCulture), camera.transform.position.z);

        return base.Execute(camera, gameObjects, ff);


    }

}