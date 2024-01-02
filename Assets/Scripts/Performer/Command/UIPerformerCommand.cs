using System.Collections.Generic;
using UnityEngine;

public class UIPerformerCommand : OneShotPerformerCommand {
    public UIPerformerCommand()
    {
        this.command = PerformerCommandEnum.UI;
    }

    public override PerformerTime Execute(Camera camera, Dictionary<string, GameObject> gameObjects, bool ff)
    {
        
        if(arguments[0].Equals("show")) {
            UIManager.GetInstance().ShowTopUI();
        } else if(arguments[0].Equals("hide")) {
            UIManager.GetInstance().HideTopUI();
        }
        
        return base.Execute(camera, gameObjects, ff);

    }

}