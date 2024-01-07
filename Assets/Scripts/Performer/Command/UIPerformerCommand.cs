using System.Collections.Generic;
using UnityEngine;

public class UIPerformerCommand : OneShotPerformerCommand {
    public UIPerformerCommand()
    {
        this.command = PerformerCommandEnum.UI;
    }

    public override PerformerTime Execute(Camera camera, Dictionary<string, GameObject> gameObjects, bool ff, bool firstTime = true)
    {

        switch(arguments[0]) {
            case "show":
                UIManager.GetInstance().ShowTopUI();
                break;
            case "hide":
                UIManager.GetInstance().HideTopUI();
                break;
            case "timer":
                GameManager.GetInstance().MakeHaste();
                break;
            default: break;
        }
        
        return base.Execute(camera, gameObjects, ff);

    }

}