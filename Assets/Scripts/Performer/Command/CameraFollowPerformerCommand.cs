using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPerformerCommand : OneShotPerformerCommand {
    public CameraFollowPerformerCommand()
    {
        this.command = PerformerCommandEnum.CAMF;
    }

    public override PerformerTime Execute(Camera camera, Dictionary<string, GameObject> gameObjects, bool ff, bool firstTime = true)
    {
        
        camera.TryGetComponent(out FollowPlayer followPlayer);
        if(followPlayer) {
            followPlayer.inCutscene = !bool.Parse(arguments[0]);
        }

        return base.Execute(camera, gameObjects, ff);

    }

}