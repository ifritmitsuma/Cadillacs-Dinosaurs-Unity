using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class PerformerScript : MonoBehaviour, IPauseListener, ICutsceneListener
{

    public string script;

    readonly Dictionary<string, List<PerformerCommand>> scenes = new();

    Dictionary<string, GameObject> dic;

    readonly LinkedList<PerformerCommand> queue = new();

    readonly Dictionary<PerformerCommand, PerformerTime> commandsExecuting = new();

    TimeConditionalPerformerCommand currentTimeConditionalCommand;

    bool paused = false;

    float pauseTime = 0.0f;

    float pauseStartTime = 0.0f;

    public bool fastForward = false;

    void Awake() {

        GameManager.GetInstance().RegisterPauseListener(this);
        PerformerManager.GetInstance().RegisterCutsceneListener(this);

        List<PerformerCommand> list = new();

        TextAsset text = Resources.Load<TextAsset>("Scenes/Stage1/" + script);

        foreach(string line in text.text.Split("\r\n")) {

            if(line.Trim() == "") {
                continue;
            }
            if(line.StartsWith("##")) {
                list = new();
                scenes.Add(line[2..].Trim(), list);
            } else { 
                string[] commandLine = Regex.Matches(line, @"[\""].+?[\""]|[^ ]+")
                                            .Cast<Match>()
                                            .Select(m => m.Value)
                                            .ToArray();
                for(int i = 0; i < commandLine.Length; ++i) {
                    commandLine[i] = commandLine[i].Replace("\"", "");
                }
                PerformerCommand command = PerformerCommand.CreatePerformerCommand(commandLine);
                list.Add(command);
            }

        }
    }

    public bool Has(string name) {

        return scenes.ContainsKey(name);

    }

    public bool IsRunningPerformance() {
        return queue.Count > 0;
    }

    public void StartPerformance(string name) {
        dic = new();
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
            dic.Add(player.name, player);
        }
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
            dic.Add(enemy.name, enemy);
        }

        scenes.TryGetValue(name, out List<PerformerCommand> commands);

        foreach(PerformerCommand command in commands) {
            queue.AddLast(command);
        }

    }

    void FixedUpdate() {

        if(queue.Count == 0 || paused) {
            return;
        }

        if(!commandsExecuting.ContainsKey(new DelayPerformerCommand())) {
            PerformerCommand command = queue.First();
            if(command is TimeConditionalPerformerCommand timeConditionalCommand && currentTimeConditionalCommand == null) {
                currentTimeConditionalCommand = timeConditionalCommand;
            }
            
            if(command is OneShotPerformerCommand) {
                if(command is ScenePerformerCommand sceneCommand) {
                    sceneCommand.unloadFinished = () => { 
                        commandsExecuting.Clear(); 
                    };
                }
                if(!(command is EndPerformerCommand && commandsExecuting.Count > 0)) { 
                    command.Execute(Camera.main, dic, fastForward);
                    queue.RemoveFirst();
                } 
            } else if(!commandsExecuting.ContainsKey(command)) {
                commandsExecuting.Add(command, null);
                queue.RemoveFirst();
            }
        }
        
        List<PerformerCommand> commandsToRemove = new();

        foreach(PerformerCommand executingCommand in commandsExecuting.Keys) {
            PerformerTime currentTime = executingCommand.Execute(Camera.main, dic, fastForward);
            commandsExecuting.TryGetValue(executingCommand, out PerformerTime commandTime);
            if(commandTime == null) {
                commandsExecuting[executingCommand] = currentTime;
                break;
            }
            
            if(currentTime.ended && commandTime.duration == -1) {
                commandsToRemove.Add(executingCommand);
            }
            if(executingCommand is TimeConditionalPerformerCommand && ((Time.time - pauseTime - commandTime.startTime) * (fastForward ? 3.0f : 1.0f) > commandTime.duration)) {
                commandsToRemove.Add(executingCommand);
                currentTimeConditionalCommand = null;
                pauseTime = 0.0f;

            }
        }

        foreach(PerformerCommand commandToRemove in commandsToRemove) {
            commandToRemove.CommandEnded();
            commandsExecuting.Remove(commandToRemove);
        }

    }

    public void Pause()
    {
        paused = true;
        pauseStartTime = Time.time;
    }

    public void Unpause()
    {
        paused = false;
        pauseTime += Time.time - pauseStartTime;
    }

    public void CutsceneStarted()
    {
        // Do nothing
    }

    public void CutsceneEnded()
    {
        // Do nothing
    }

    public void FastForward(bool ff)
    {
        fastForward = ff;
    }
}
