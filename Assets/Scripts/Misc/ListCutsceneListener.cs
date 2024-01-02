using System.Collections.Generic;

public class ListCutsceneListener : ICutsceneListener
{

    private List<ICutsceneListener> cutsceneListeners = new();

    public void AddCutsceneListener(ICutsceneListener cutsceneListener) {
        cutsceneListeners.Add(cutsceneListener);
    }

    public void SetCutsceneListeners(List<ICutsceneListener> cutsceneListener) {
        this.cutsceneListeners = cutsceneListener;
    }

    public void RemoveCutsceneListener(ICutsceneListener cutsceneListener) {
        cutsceneListeners.Remove(cutsceneListener);
    }
    public void CutsceneStarted()
    {
        foreach(ICutsceneListener cutsceneListener in cutsceneListeners) {
            cutsceneListener.CutsceneStarted();
        }
    }

    public void CutsceneEnded()
    {
        foreach(ICutsceneListener cutsceneListener in cutsceneListeners) {
            cutsceneListener.CutsceneEnded();
        }
    }

    public void FastForward(bool ff)
    {
        foreach(ICutsceneListener cutsceneListener in cutsceneListeners) {
            cutsceneListener.FastForward(ff);
        }
    }
}