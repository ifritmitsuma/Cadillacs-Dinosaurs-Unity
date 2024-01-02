using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPerformerCommand : ConditionalPerformerCommand {
    public ObjectPerformerCommand()
    {
        this.command = PerformerCommandEnum.OBJ;
    }

    public override PerformerTime Execute(Camera camera, Dictionary<string, GameObject> gameObjects, bool ff)
    {

        PerformerTime time = base.Execute(camera, gameObjects, ff);
        time.duration = -1;

        GameObject obstacle = Array.Find<GameObject>(GameObject.FindGameObjectsWithTag("Obstacle"), (x) => x.name == arguments[0]);

        if(obstacle == null) {
            time.ended = true;
            return time;
        }

        string[] animArgs = arguments[2..];
        obstacle.TryGetComponent<Obstacle>(out var obstacleScript);
        if(!obstacleScript.Animate(arguments[1], animArgs)) {
            return time;
        }
        time.ended = true;
        return time;

    }
    


}