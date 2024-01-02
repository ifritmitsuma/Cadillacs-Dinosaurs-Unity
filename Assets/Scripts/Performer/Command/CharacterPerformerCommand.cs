using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPerformerCommand : ConditionalPerformerCommand {
    public CharacterPerformerCommand()
    {
        this.command = PerformerCommandEnum.CHAR;
    }

    public override PerformerTime Execute(Camera camera, Dictionary<string, GameObject> gameObjects, bool ff)
    {


        PerformerTime time = base.Execute(camera, gameObjects, ff);
        time.duration = -1;

        gameObjects.TryGetValue(arguments[0], out GameObject character);
        if(character == null) {
            time.ended = true;
            return time;
        }
        string[] animArgs = arguments[2..];
        character.TryGetComponent<Character>(out var characterScript);
        if(!characterScript.Animate(arguments[1], animArgs)) {
            return time;
        }
        time.ended = true;
        return time;

    }
    


}