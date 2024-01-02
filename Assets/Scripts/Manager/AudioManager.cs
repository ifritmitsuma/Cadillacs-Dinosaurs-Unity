using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioManager : MonoBehaviour, IPauseListener
{

    private static AudioManager instance;

    public AudioSource musicAudioSource;
    
    public AudioSource sfxAudioSource;

    private Dictionary<string, AudioClip> clips = new();

    private int totalClips = 0;

    private bool paused;

    private bool bundleLoaded;

    public static AudioManager GetInstance() {
        return instance;
    }

    void Awake() {
        
        instance = this;

        GameManager.GetInstance().RegisterPauseListener(this);
    }

    public void LoadBundle(string bundleFileName) {
        bundleLoaded = false;
        UIManager.GetInstance().ShowLoading();
        AssetBundleCreateRequest clipsBundle = AssetBundle.LoadFromFileAsync(Application.streamingAssetsPath.ToString() + "/" + bundleFileName);
        StartCoroutine(LoadBundle(clipsBundle));
    }

    private IEnumerator LoadBundle(AssetBundleCreateRequest request) {
        yield return new WaitUntil(() => {
            UIManager.GetInstance().UpdateLoading(request.progress, request.isDone);
            return request.isDone;
        });
        AudioClip[] clipArray = request.assetBundle.LoadAllAssets<AudioClip>();
    	totalClips = clipArray.Length;
        UIManager.GetInstance().ShowLoading();
        foreach(AudioClip clip in clipArray) {
            clip.LoadAudioData();
            StartCoroutine(LoadClip(clip));
        }
    }

    private IEnumerator LoadClip(AudioClip clip) {
        yield return new WaitUntil(() => {
            return clip.loadState == AudioDataLoadState.Loaded;
        });
        clips.Add(clip.name, clip);
        UIManager.GetInstance().UpdateLoading(clips.Count / totalClips, clips.Count == totalClips);
        if(clips.Count == totalClips) {
            bundleLoaded = true;
        }
    }

    public void PlayMusic(string name, bool loop) {
        PlayMusic(GetClip(name), loop);
    }

    public void PlayMusic(AudioClip clip, bool loop) {
        musicAudioSource.clip = clip;
        musicAudioSource.loop = loop;
        clip.LoadAudioData();
        StartCoroutine(Play());
    }

    private IEnumerator Play() {
        yield return new WaitUntil(() => {
            return musicAudioSource.clip.loadState == AudioDataLoadState.Loaded;
        });
        musicAudioSource.Play();
    }

    public AudioClip GetClip(string name)
    {
        instance.clips.TryGetValue(name, out AudioClip clip);
        if(clip == null) {
            throw new SystemException("Audio clip with name '" + name + "' not found!");
        }
        return clip;
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }

    internal void PlaySound(AudioClip clip)
    {
        if(!paused)
            sfxAudioSource.PlayOneShot(clip);
    }

    public void Pause()
    {
        paused = true;
        musicAudioSource.Pause();
        sfxAudioSource.Pause();
    }

    public void Unpause()
    {
        paused = false;
        musicAudioSource.UnPause();
        sfxAudioSource.UnPause();
    }

    public bool IsBundleLoaded()
    {
        return bundleLoaded;
    }
}
