
using System;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

public enum PerformerCommandEnum {

    PLAY, CAMM, TEXT, CHAR, END, DELAY, UI, OBJ, SCENE, CAMS, NEWC, CAMF

}

public static class PerformerCommandEnumExtensions
{

    public static PerformerCommandEnum GetByName(string name) {
        return name switch
        {
            "play" => PerformerCommandEnum.PLAY,
            "cams" => PerformerCommandEnum.CAMS,
            "camm" => PerformerCommandEnum.CAMM,
            "text" => PerformerCommandEnum.TEXT,
            "char" => PerformerCommandEnum.CHAR,
            "end" => PerformerCommandEnum.END,
            "delay" => PerformerCommandEnum.DELAY,
            "ui" => PerformerCommandEnum.UI,
            "obj" => PerformerCommandEnum.OBJ,
            "scene" => PerformerCommandEnum.SCENE,
            "newc" => PerformerCommandEnum.NEWC,
            "camf" => PerformerCommandEnum.CAMF,
            _ => throw new SystemException("PerformerCommand unknown!")
        };
    }

    public static PerformerCommand NewPerformerCommand(string command)
    {
        PerformerCommandEnum pCEnum = GetByName(command);

        return pCEnum switch {
            PerformerCommandEnum.PLAY => new PlayPerformerCommand(),
            PerformerCommandEnum.CAMS => new CameraSetPerformerCommand(),
            PerformerCommandEnum.CAMM => new CameraMovePerformerCommand(),
            PerformerCommandEnum.TEXT => new TextPerformerCommand(),
            PerformerCommandEnum.CHAR => new CharacterPerformerCommand(),
            PerformerCommandEnum.END => new EndPerformerCommand(),
            PerformerCommandEnum.DELAY => new DelayPerformerCommand(),
            PerformerCommandEnum.UI => new UIPerformerCommand(),
            PerformerCommandEnum.OBJ => new ObjectPerformerCommand(),
            PerformerCommandEnum.SCENE => new ScenePerformerCommand(),
            PerformerCommandEnum.NEWC => new NewCharacterPerformerCommand(),
            PerformerCommandEnum.CAMF => new CameraFollowPerformerCommand(),
            _ => throw new SystemException("PerformerCommand unknown!")
        };

    }
}