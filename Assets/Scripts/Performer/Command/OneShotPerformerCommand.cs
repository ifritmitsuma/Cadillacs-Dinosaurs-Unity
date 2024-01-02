using System.Collections.Generic;
using UnityEngine;

public abstract class OneShotPerformerCommand : PerformerCommand {

    public override PerformerTime Execute(Camera camera, Dictionary<string, GameObject> dic, bool ff) {
        PerformerTime time = base.Execute(camera, dic, ff);
        time.ended = true;
        return time;
    }

    public override void CommandEnded() {
        // Do nothing;
    }

}