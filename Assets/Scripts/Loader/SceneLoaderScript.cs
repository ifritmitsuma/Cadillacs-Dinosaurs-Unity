using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderScript : MonoBehaviour
{

    private static SceneLoaderScript instance;

    public static SceneLoaderScript GetInstance() {
        return instance;
    }

    private string currentScene;

    void Start() {
        instance = this;
        AudioManager.GetInstance().LoadBundle("stage1musicbundle");
        StartCoroutine(LoadMusicBundle());
    }

    IEnumerator LoadMusicBundle() {

        yield return new WaitUntil(() => {
            return AudioManager.GetInstance().IsBundleLoaded();
            }
        );
        LoadScene("Roof");

    }

    public void LoadScene(string scene, Action unloadFinished = null)
    {
        if(scene == currentScene) {
            return;    
        }
        
        UIManager.GetInstance().ShowLoading();
        if(currentScene != null) {
            StartCoroutine(UnloadSceneAsync(scene, unloadFinished));
        } else {
            StartCoroutine(LoadSceneAsync(scene));
        }
    }

    IEnumerator UnloadSceneAsync(string scene = null, Action unloadFinished = null) {

        AsyncOperation unloadSceneOp = SceneManager.UnloadSceneAsync(currentScene);
        yield return new WaitUntil(() => {
            UIManager.GetInstance().UpdateLoading(unloadSceneOp.progress / (scene != null ? 2 : 1), unloadSceneOp.isDone && scene == null);
            if(unloadSceneOp.isDone) {
                return true;
            }
            return false;
        });

        unloadFinished?.Invoke();
        
        if(scene != null) {
            StartCoroutine(LoadSceneAsync(scene, true));
        }

    }

    IEnumerator LoadSceneAsync(string scene, bool unloadBefore = false) {

        AsyncOperation loadSceneOp = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        yield return new WaitUntil(() => {
            UIManager.GetInstance().UpdateLoading(loadSceneOp.progress / (unloadBefore ? 2 : 1), loadSceneOp.isDone);
            if(loadSceneOp.isDone) {
                return true;
            }
            return false;
        });
        currentScene = scene;

    }

}
