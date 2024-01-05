using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private List<string> playersSelected = new(2) { "Mustapha" };

    private Dictionary<int, PlayerStats> playerStats = new(2);

    private List<Player> players = new();

    private static GameManager instance;

    public static GameManager GetInstance() {
        return instance;
    }

    private readonly ListPauseListener pauseListeners = new();

    public void RegisterPauseListener(IPauseListener pauseListener)
    {
        pauseListeners.AddPauseListener(pauseListener);
    }

    public void UnregisterPauseListener(IPauseListener pauseListener)
    {
        pauseListeners.RemovePauseListener(pauseListener);
    }

    private bool paused;

    private bool timerEnabled = false;

    private readonly float defaultTimer = 65.0f;

    private float timer = 0.0f; 

    void Awake() {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        bool pauseButton = Input.GetButtonDown("Pause");

        if(pauseButton && !UIManager.GetInstance().IsLoading()) {
            paused = !paused;
            if(paused) {
                pauseListeners.Pause();
            } else {
                pauseListeners.Unpause();
            }
        }

        if(!paused) {
            UpdateTimer();
        }
        
    }

    private void UpdateTimer()
    {
        if(timerEnabled) {
            timer -= Time.deltaTime;
            if(timer <= 0.0f) {
                timerEnabled = false;
                foreach(Player player in players) {
                    KillCharacter(player, playerStats[player.index], player.transform.position);
                }
                UIManager.GetInstance().DisplayText("center", "OUT OF TIME");
                return;
            } else if((timer % 30 > 24.0f && timer % 30 < 30.0f) || timer < 11.0f) {
                UIManager.GetInstance().DisplayText("center", TimeSpan.FromSeconds(timer).Minutes.ToString() + ":" + string.Format("{0:D2}", TimeSpan.FromSeconds(timer).Seconds));
            } else {
                UIManager.GetInstance().HideMessage("center");
            }
            if(((int)timer) % 10 == 9) {
                UIManager.GetInstance().ShowGoAnimation();
            }
        }
    }

    public GameObject LoadPlayer(int index, PlayerStats stats = null)
    {
        GameObject playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        GameObject player = Instantiate(playerPrefab);
        player.name = "Player" + index;
        Player playerScript = player.GetComponent<Player>();
        playerScript.index = index;
        if(stats != null) {
            playerStats[index] = stats;
        } else {
            playerStats[index] = new PlayerStats() {hp = 100, lives = 1, score = 0};
        }
        players.Add(playerScript);
        return player;
    }

    public GameObject LoadPlayer(int index, Vector3 position, PlayerStats stats = null)
    {
        GameObject player = LoadPlayer(index, stats);
        player.transform.position = position;
        return player;
    }

    public void AddPlayerInfo(GameObject player, PlayerInfo info) {
        Player playerScript = player.GetComponent<Player>();
        playerScript.info = info;
        UIManager.GetInstance().SetPlayerInfo(playerScript.index, info, playerStats[playerScript.index]);
    }

    public void AddPlayerSounds(GameObject player, PlayerSoundSelection playerSounds)
    {
        PlayerAnimation playerAnimationScript = player.GetComponentInChildren<PlayerAnimation>();
        playerAnimationScript.playerSounds = playerSounds;
    }

    public PlayerSoundSelection GetPlayerSounds(Player player)
    {
        return player.GetComponentInChildren<PlayerAnimation>().playerSounds;
    }

    public PlayerInfo GetPlayerInfo(Player player)
    {
        return player.info;
    }

    public void UpdateHP(Player player, int amount)
    {
        PlayerStats stats = playerStats[player.index];
        stats.hp = Mathf.Min(Mathf.Max(0, stats.hp + amount), 100);
        if(stats.hp == 0) {
            Camera.main.GetComponent<FollowPlayer>().locked = true;
            Vector3 lastPosition = player.transform.position;
            transform.position = player.transform.position;
            KillCharacter(player, stats, lastPosition);
        }
        UIManager.GetInstance().UpdateHP(player.index, stats.hp);
    }

    public void UpdateScore(Player player, int amount)
    {
        PlayerStats stats = playerStats[player.index];
        stats.score += amount;
        UIManager.GetInstance().UpdateScore(player.index, amount);
    }

    IEnumerator AfterDeath(Character character, PlayerStats stats = null, Vector3? lastPosition = null)
    {
        yield return new WaitUntil(() => character.Die());
        Destroy(character.gameObject);
        switch(character) {
            case Player:
                Player player = (Player) character;
                players.Remove(player);
                if(stats.lives > 0) {
                    Camera.main.GetComponent<FollowPlayer>().locked = false;
                    stats.lives--;
                    stats.hp = 100;
                    PlayerInfo playerInfo = GetPlayerInfo(player);
                    PlayerSoundSelection playerSounds = GetPlayerSounds(player);
                    GameObject newPlayer = LoadPlayer(player.index, lastPosition.Value, stats);
                    AddPlayerInfo(newPlayer, playerInfo);
                    AddPlayerSounds(newPlayer, playerSounds);
                    UIManager.GetInstance().SetPlayerInfo(player.index, player.info, stats);
                    UIManager.GetInstance().HideMessage("center");
                    MakeHaste();
                } else {
                    UIManager.GetInstance().GameOver(player.index);
                }
                break;
            case Enemy:
                break;
            default: break;
        }
    }

    public void KillCharacter(Character character, PlayerStats stats = null, Vector3? lastPosition = null)
    {
        StartCoroutine(AfterDeath(character, stats, lastPosition));
    }

    public void MakeHaste(float timerValue = -1.0f)
    {
        if(!timerEnabled) {
            timerEnabled = true;
            timer = timerValue == -1.0f ? defaultTimer : timerValue;
            return;
        }
        if(timerValue != -1.0f) {
            timer = timerValue;
        }
    }

    public void MakeHaste(string performerAct, FollowPlayer followPlayer = null) {
        if(followPlayer) {
            followPlayer.locked = false;
        }
        timerEnabled = false;
        UIManager.GetInstance().ShowGoAnimation(PerformerManager.GetInstance().RunPerformance, performerAct);
    }

    public void ClearTimer() {
        timerEnabled = false;
        timer = 0.0f;
    }

    public void LoadPlayers(Vector3 player1Pos, Vector3 player2Pos)
    {

        if(players.Count > 0) {
            return;
        }

        if(playersSelected.Count == 1) {
            player1Pos += (player2Pos - player1Pos) / 2;
        }

        Vector3[] playerPositions = { player1Pos, player2Pos };

        for(int i = 0; i < playersSelected.Count; ++i) {
            
            GameObject player = instance.LoadPlayer(i + 1, playerPositions[i]);

            PlayerInfo playerInfo = Resources.Load<PlayerInfo>("ScriptableObjects/Info/Player/" + playersSelected[i]);
            PlayerSoundSelection playerSounds = Resources.Load<PlayerSoundSelection>("ScriptableObjects/Sound/Player/" + playersSelected[i]);
            instance.AddPlayerInfo(player, playerInfo);
            instance.AddPlayerSounds(player, playerSounds);

        }

    }
}
