using System.Collections.Generic;

public class ListPauseListener : IPauseListener {

    private List<IPauseListener> pauseListeners = new();

    public void AddPauseListener(IPauseListener pauseListener) {
        pauseListeners.Add(pauseListener);
    }

    public void SetPauseListeners(List<IPauseListener> pauseListeners) {
        this.pauseListeners = pauseListeners;
    }

    public void RemovePauseListener(IPauseListener pauseListener) {
        pauseListeners.Remove(pauseListener);
    }

    public void Pause()
    {
        foreach(IPauseListener pauseListener in pauseListeners) {
            pauseListener.Pause();
        }
    }

    public void Unpause()
    {
        foreach(IPauseListener pauseListener in pauseListeners) {
            pauseListener.Unpause();
        }
    }
}