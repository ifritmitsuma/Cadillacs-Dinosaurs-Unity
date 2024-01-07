using System;
using UnityEngine;

public class PerformerManager : MonoBehaviour
{

    private static PerformerManager instance;

    public static PerformerManager GetInstance()
    {
        return instance;
    }

    private readonly ListCutsceneListener cutsceneListeners = new();

    
    public void RegisterCutsceneListener(ICutsceneListener cutsceneListener)
    {
        cutsceneListeners.AddCutsceneListener(cutsceneListener);
    }

    public void UnregisterCutsceneListener(ICutsceneListener cutsceneListener)
    {
        cutsceneListeners.RemoveCutsceneListener(cutsceneListener);
    }

    bool ff;

    private PerformerScript performer;

    public void SetPerformer(PerformerScript performer) {
        this.performer = performer;
        RunPerformance("Start");
    }

    void Awake() {
        instance = this;
    }

    void Update() {
        
        bool ffNew = Input.GetButton("FastForward");

        if(ffNew != ff && performer != null && IsRunningPerformance()) {
            cutsceneListeners.FastForward(ffNew);
        } 

        ff = ffNew;

    }

    public bool HasPerformance(String name) {
        return performer.Has(name);
    }

    public void RunPerformance(String name) {
        if(HasPerformance(name)) {
            performer.StartPerformance(name);
            cutsceneListeners.CutsceneStarted();
        }
    }

    public bool IsRunningPerformance() {
        return performer.IsRunningPerformance();
    }

    public void CutsceneEnded() {
        cutsceneListeners.CutsceneEnded();
        if(ff) {
            cutsceneListeners.FastForward(false);
        }
    }

    internal void AddGameObject(GameObject obj)
    {
        if(performer == null) {
            return;
        }
        performer.AddGameObject(obj);
    }
}
