using System.Collections.Generic;
using UnityEngine;

public abstract class ConditionalPerformerCommand : PerformerCommand {
    
    public override PerformerTime Execute(Camera camera, Dictionary<string, GameObject> dic, bool ff) {
        return new();
    }

    public override void CommandEnded() {
        // Do nothing;
    }

}