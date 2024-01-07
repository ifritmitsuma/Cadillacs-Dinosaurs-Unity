using System.Collections.Generic;
using UnityEngine;

public class EndPerformerCommand : OneShotPerformerCommand {
    public EndPerformerCommand()
    {
        this.command = PerformerCommandEnum.END;
    }

    public override PerformerTime Execute(Camera camera, Dictionary<string, GameObject> gameObjects, bool ff, bool firstTime = true)
    {
        UIManager.GetInstance().HideAllMessages();

        PerformerManager.GetInstance().CutsceneEnded();

        GameManager.GetInstance().MakeHaste();

        return base.Execute(camera, gameObjects, ff);

    }

}