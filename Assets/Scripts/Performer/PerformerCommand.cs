using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public abstract class PerformerCommand {

    public static PerformerCommand CreatePerformerCommand(string[] commandLine)
    {
        PerformerCommand performerCommand = PerformerCommandEnumExtensions.NewPerformerCommand(commandLine[0]);
        performerCommand.arguments = new string[commandLine.Length - 1];
        if(commandLine.Length > 1) {
            Array.Copy(commandLine, 1, performerCommand.arguments, 0, performerCommand.arguments.Length);
        }
        return performerCommand;
    }

    protected PerformerCommandEnum command {
        get; set;
    }

    protected string[] arguments {
        get; set;
    }

    public virtual PerformerTime Execute(Camera camera, Dictionary<string, GameObject> gameObjects, bool ff, bool firstTime = true) {

        return new() {
            startTime = Time.time
        };

    }

    public abstract void CommandEnded();

    // override object.Equals
    public override bool Equals(object obj)
    {
        //
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //
        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        PerformerCommand other = (PerformerCommand) obj;
        if(command != other.command) {
            return false;
        }

        return command == PerformerCommandEnum.CHAR ? command + arguments[0] == other.command + other.arguments[0] : command == other.command; 
    
    }
    
    // override object.GetHashCode
    public override int GetHashCode()
    {
        return command == PerformerCommandEnum.CHAR ? command.GetHashCode() + arguments[0].GetHashCode() : command.GetHashCode();
    }

}