using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class TextPerformerCommand : TimeConditionalPerformerCommand {
    public TextPerformerCommand()
    {
        this.command = PerformerCommandEnum.TEXT;
    }

    public override PerformerTime Execute(Camera camera, Dictionary<string, GameObject> gameObjects, bool ff)
    {
        PerformerTime time = base.Execute(camera, gameObjects, ff);

        UIManager.GetInstance().DisplayText(arguments[0], arguments[1]);
        time.duration = float.Parse(arguments[2], CultureInfo.InvariantCulture);
        return time;

    }

    public override void CommandEnded() {
        UIManager.GetInstance().HideMessage(arguments[0]);
    }
    
                
    
}