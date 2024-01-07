using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class DelayPerformerCommand : TimeConditionalPerformerCommand {
    public DelayPerformerCommand()
    {
        this.command = PerformerCommandEnum.DELAY;
    }

    public override PerformerTime Execute(Camera camera, Dictionary<string, GameObject> gameObjects, bool ff, bool firstTime = true)
    {
        PerformerTime time = base.Execute(camera, gameObjects, ff);
        time.duration = float.Parse(arguments[0], CultureInfo.InvariantCulture);
        return time;

    }
    
                
    
}