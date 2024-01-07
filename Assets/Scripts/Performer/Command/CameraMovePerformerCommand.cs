using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CameraMovePerformerCommand : ConditionalPerformerCommand {

    private float initialX;

    private bool initialXObtained = false;

    public CameraMovePerformerCommand()
    {
        this.command = PerformerCommandEnum.CAMM;
    }

    public override PerformerTime Execute(Camera camera, Dictionary<string, GameObject> gameObjects, bool ff, bool firstTime = true)
    {

        PerformerTime time = base.Execute(camera, gameObjects, ff);

        Vector3 previousPos = camera.transform.position;
        if(!initialXObtained) {
            initialX = camera.transform.position.x;
            initialXObtained = true;
        }
        camera.transform.position = Vector3.MoveTowards(camera.transform.position, new Vector3(initialX + float.Parse(arguments[0], CultureInfo.InvariantCulture), camera.transform.position.y, camera.transform.position.z), float.Parse(arguments[1], CultureInfo.InvariantCulture) * (ff ? 3.0f : 1.0f) * Time.deltaTime);
        if(previousPos == camera.transform.position) {
            time.ended = true;
        }
        time.duration = -1;
        return time;

    }

}