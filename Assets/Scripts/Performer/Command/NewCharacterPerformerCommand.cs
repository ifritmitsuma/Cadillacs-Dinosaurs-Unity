using System.Collections.Generic;
using UnityEngine;

public class NewCharacterPerformerCommand : OneShotPerformerCommand {
    public NewCharacterPerformerCommand()
    {
        this.command = PerformerCommandEnum.NEWC;
    }

    public override PerformerTime Execute(Camera camera, Dictionary<string, GameObject> gameObjects, bool ff)
    {
        
        GameObject gameObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/" + arguments[0]), new Vector2(float.Parse(arguments[2]), float.Parse(arguments[3])), Quaternion.identity);
        gameObject.name = arguments[1];
        gameObject.transform.Find("Animation").transform.localScale = new Vector3(arguments[4] == "right" ? 1 : -1, 1, 1);
        gameObjects.Add(arguments[1], gameObject);

        return base.Execute(camera, gameObjects, ff);

    }

}