using System;
using System.Collections;
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

    private static UIManager instance;

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

    public Transform GetPlayerUI(int index) {
        return topUI.transform.Find("Player" + index);
    }

    public void SetPlayerInfo(int player, PlayerInfo info, PlayerStats playerStats) {

        if(player < 1 || player > 2) {
            return;
        }

        Transform playerInfo = GetPlayerUI(player).Find("Info");
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

        Transform playerInfo = GetPlayerUI(player).Find("Info");
        Slider hpSlider = playerInfo.Find("HP").GetComponent<Slider>();
        hpSlider.value = hp;

    }

    public void UpdateLives(int player, int lives) {

        if(player < 0 || player > 1) {
            return;
        }

        Transform playerInfo = GetPlayerUI(player).Find("Info");
        Text livesText = playerInfo.Find("Lives").GetComponent<Text>();
        livesText.text = "=" + lives;
        
    }

    public void UpdateScore(int player, int score) {
        
        if(player < 0 || player > 1) {
            return;
        }

        Transform playerInfo = GetPlayerUI(player).Find("Info");
        Text scoreText = playerInfo.Find("Score").GetComponent<Text>();
        scoreText.text = string.Format("{0:D7}", score);
    }

    public void GameOver(int index)
    {
        Transform player = GetPlayerUI(index);
        player.Find("Info").gameObject.SetActive(false);
        Transform status = player.Find("Status");
        status.gameObject.SetActive(true);
        status.GetComponent<Text>().text = "GAME OVER";
        
    }

    public void ShowGoAnimation(Action<string> callback = null, string argument = null)
    {
        if(!goAnimator.GetCurrentAnimatorStateInfo(0).IsName("Go"))
            goAnimator.Play("Go");

        if(callback != null) {
            StartCoroutine(AfterGoAnimation(callback, argument));
        }
    }

    IEnumerator AfterGoAnimation(Action<string> callback, string argument)
    {
        yield return new WaitUntil(() => !goAnimator.GetCurrentAnimatorStateInfo(0).IsName("Go"));
        callback.Invoke(argument);
    }
}
