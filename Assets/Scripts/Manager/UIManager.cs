using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IPauseListener
{

    public Canvas uiCanvas;

    public Slider loading;

    private int loadingCount;

    public Canvas pauseScreen;

    public Canvas topUI;

    public Animator goAnimator;

    public Image fadeOverlay;

    private static UIManager instance;

    private readonly Dictionary<int, Coroutine> uiTargetCoRoutines = new();

    public static UIManager GetInstance()
    {
        return instance;
    }

    public void DisplayText(string placement, string text)
    {
        uiCanvas.transform.Find(placement).GetComponent<Text>().text = text;
    }

    void Awake() {
        instance = this;

        GameManager.GetInstance().RegisterPauseListener(this);
    }

    public void HideMessage(string placement) {
        uiCanvas.transform.Find(placement).GetComponent<Text>().text = "";
    }

    public void HideAllMessages()
    {
        HideMessage("center");
        HideMessage("bottom");
    }

    public void ShowLoading() {
        loadingCount++;
        loading.gameObject.SetActive(true);
    }

    public void UpdateLoading(float progress, bool done = false)
    {
        loading.value = progress;
        if(progress == 1.0f || done) {
            loadingCount--;
        }
        if(loadingCount == 0) {
            HideLoading();
        }
    }

    public void HideLoading()
    {
        loading.gameObject.SetActive(false);
    }

    public bool IsLoading()
    {
        return loadingCount > 0 || loading.gameObject.activeSelf;
    }

    public void Pause()
    {
        pauseScreen.gameObject.SetActive(true);
    }

    public void Unpause()
    {
        pauseScreen.gameObject.SetActive(false);
    }

    public void ShowTopUI() {
        topUI.gameObject.SetActive(true);
    }

    public void HideTopUI() {
        topUI.gameObject.SetActive(false);
    }

    public bool IsTopUIShown() {
        return topUI.gameObject.activeSelf;
    }

    public Transform GetPlayerUITop(int index) {
        return topUI.transform.Find("Player" + index);
    }

    public void SetPlayerInfo(int player, PlayerInfo info, PlayerStats playerStats) {

        if(player < 1 || player > 2) {
            return;
        }

        Transform playerInfo = GetPlayerUITop(player).Find("Info");
        playerInfo.gameObject.SetActive(true);
        Image avatar = playerInfo.Find("Picture").GetComponent<Image>();
        Text nameText = playerInfo.Find("Name").GetComponent<Text>();

        if(info.avatar) {
            avatar.sprite = info.avatar;
        }
        if(info.name != null) {
            nameText.text = info.name.ToUpper();
        }
        UpdateHP(player, playerStats.hp);
        UpdateLives(player, playerStats.lives);
        UpdateScore(player, playerStats.score);
 
    }

    public void UpdateHP(int player, int hp) {

        if(player < 0 || player > 1) {
            return;
        }

        Transform playerInfo = GetPlayerUITop(player).Find("Info");
        Slider hpSlider = playerInfo.Find("HP").GetComponent<Slider>();
        hpSlider.value = hp;

    }

    public void UpdateLives(int player, int lives) {

        if(player < 0 || player > 1) {
            return;
        }

        Transform playerInfo = GetPlayerUITop(player).Find("Info");
        Text livesText = playerInfo.Find("Lives").GetComponent<Text>();
        livesText.text = "=" + lives;
        
    }

    public void UpdateScore(int player, int score) {
        
        if(player < 0 || player > 1) {
            return;
        }

        Transform playerInfo = GetPlayerUITop(player).Find("Info");
        Text scoreText = playerInfo.Find("Score").GetComponent<Text>();
        scoreText.text = string.Format("{0:D7}", score);
    }

    public void GameOver(int index)
    {
        Transform player = GetPlayerUITop(index);
        player.Find("Info").gameObject.SetActive(false);
        Transform status = player.Find("Status");
        status.gameObject.SetActive(true);
        status.GetComponent<Text>().text = "GAME OVER";
        
    }

    public void ShowGoAnimation(Action<string> callback = null, string argument = null)
    {
        if(!AnimationManager.GetInstance().IsAnimationPlaying(goAnimator, "Go"))
            AnimationManager.GetInstance().Play(goAnimator, "Go");

        if(callback != null) {
            StartCoroutine(AfterGoAnimation(callback, argument));
        }
    }

    IEnumerator AfterGoAnimation(Action<string> callback, string argument)
    {
        yield return new WaitUntil(() => !AnimationManager.GetInstance().IsAnimationPlaying(goAnimator, "Go"));
        callback.Invoke(argument);
    }

    public void OverlayFade(bool fadeIn, Action callback = null) {
        StartCoroutine(OverlayFadeCoroutine(fadeIn, callback)); 
    }

    IEnumerator OverlayFadeCoroutine(bool fadeIn, Action callback = null)
    {
        yield return new WaitUntil(() => {
            fadeOverlay.color = new Color(fadeOverlay.color.r, fadeOverlay.color.g, fadeOverlay.color.b, Mathf.Max(0, Mathf.Min(fadeOverlay.color.a + (fadeIn ? -0.01f : 0.01f), 1)));
            return fadeOverlay.color.a == (fadeIn ? 0 : 1);
        });
        callback?.Invoke();
    }

    public void NewPickup(int playerIndex, Item item)
    {

        switch(item) {
            case Weapon:
                break;
            case Food:
            case Loot:
                if(uiTargetCoRoutines.ContainsKey(playerIndex)) {
                    StopCoroutine(uiTargetCoRoutines[playerIndex]);
                    uiTargetCoRoutines.Remove(playerIndex);
                }
                SetPlayerUITopTarget(playerIndex, item.uiIcon, item.itemInfo.name, item is Food ? "TASTY !!!" : ((LootInfo) item.itemInfo).score + " Pts");
                uiTargetCoRoutines.Add(playerIndex, StartCoroutine(ClearPlayerTargetUI(playerIndex)));
                break;
            default: break;
        }
    }

    IEnumerator ClearPlayerTargetUI(int playerIndex)
    {
        yield return new WaitForSeconds(3);
        SetPlayerUITopTarget(playerIndex, null, null);
        uiTargetCoRoutines.Remove(playerIndex);
        
    }

    void SetPlayerUITopTarget(int playerIndex, Sprite icon, string name, string text = null, int hp = -1) {

        Transform topPlayer = GetPlayerUITop(playerIndex);
        topPlayer.Find("Target/Icon").GetComponent<Image>().sprite = icon;
        topPlayer.Find("Target/Name").GetComponent<Text>().text = name;
        topPlayer.Find("Target/Text").gameObject.SetActive(text != null);
        topPlayer.Find("Target/Text").GetComponent<Text>().text = text;
        topPlayer.Find("Target/HP").gameObject.SetActive(hp > -1);
        topPlayer.Find("Target/HP").GetComponent<Slider>().value = hp;
        topPlayer.Find("Target").gameObject.SetActive(name != null);
    
    }
}
