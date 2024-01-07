using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class NewCharacterPerformerCommand : OneShotPerformerCommand {
    public NewCharacterPerformerCommand()
    {
        this.command = PerformerCommandEnum.NEWC;
    }

    public override PerformerTime Execute(Camera camera, Dictionary<string, GameObject> gameObjects, bool ff, bool firstTime = true)
    {
        
        GameObject gameObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/" + arguments[0]), new Vector2(float.Parse(arguments[2], CultureInfo.InvariantCulture), float.Parse(arguments[3], CultureInfo.InvariantCulture)), Quaternion.identity);
        gameObject.name = arguments[1];
        gameObject.GetComponent<Character>().CutsceneStarted();
        gameObject.transform.localScale = new Vector3(arguments[4] == "right" ? 1 : -1, 1, 1);
        gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = gameObjects.Count;
        gameObjects.Add(arguments[1], gameObject);

        return base.Execute(camera, gameObjects, ff);

    }

}