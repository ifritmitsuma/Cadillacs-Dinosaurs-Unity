using System.Collections.Generic;
using UnityEngine;

public class PlayPerformerCommand : OneShotPerformerCommand {
    public PlayPerformerCommand()
    {
        this.command = PerformerCommandEnum.PLAY;
    }

    public override PerformerTime Execute(Camera camera, Dictionary<string, GameObject> gameObjects, bool ff)
    {
        AudioManager.GetInstance().PlayMusic(arguments[0], false);

        return base.Execute(camera, gameObjects, ff);

    }

}