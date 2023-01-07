using BumperBallGame;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class InGameMenu : MonoBehaviour
{

    [Tooltip("Win game message")]
    public string WinGameMessage;

    [Tooltip("Duration of delay before the win message")]
    public float DelayBeforeWinMessage = 2f;

    [Tooltip("Pause button")]
    public GameObject PauseButton;
    public GameObject PauseMenu;
    [Tooltip("Restart button")]
    public GameObject RestartButton;
    [Tooltip("Exit button")]
    public GameObject ExitButton;
    [Tooltip("Win Text UI")]
    public GameObject WinText;
    public GameObject PlayerScoreText;
    public GameObject PlayerNameText;
    public GameObject ScoreText;
    public GameModeManager GameModeManager;

    void Awake()
    {
        //EventManager.AddListener<PlayerOutOfLivesEvent>(OnPlayerOutOfLives);
        EventManager.AddListener<GameOverEvent>(OnGameOver);
        EventManager.AddListener<UpdateScoreEvent>(OnUpdateScore);    
    }

    private void Start()
    {
        GameModeManager = GetComponent<GameModeManager>();
        string scores = "";
        string players = "";
        foreach (GameObject p in GameModeManager.players)
        {
            scores += GameModeManager.scores[p] + "\n";
            players += p.transform.parent.name  + "\n";
        }
        PlayerScoreText.GetComponent<TMP_Text>().SetText(scores);
        PlayerNameText.GetComponent<TMP_Text>().SetText(players);
    }


    /*void OnPlayerOutOfLives(PlayerOutOfLivesEvent evt)
    {
        if (evt.Killed.GetComponent<PlayerController>())
        {
            RestartButton.SetActive(true);
            ExitButton.SetActive(true);
        }
    }*/

    void OnGameOver(GameOverEvent evt)
    {
        WinText.GetComponent<TMP_Text>().SetText(evt.Winner.transform.parent.name + " " + WinGameMessage);
        RestartButton.SetActive(true);
        ExitButton.SetActive(true);
        WinText.SetActive(true);
    }

    void OnUpdateScore(UpdateScoreEvent evt)
    {
        string scores = "";
        foreach (GameObject p in GameModeManager.players)
        {
            scores += GameModeManager.scores[p] + "\n";
        }
        PlayerScoreText.GetComponent<TMP_Text>().SetText(scores);
    }

    void OnDestroy()
    {
        //EventManager.RemoveListener<PlayerOutOfLivesEvent>(OnPlayerOutOfLives);
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        EventManager.RemoveListener<UpdateScoreEvent>(OnUpdateScore);
    }

    public void HideUI()
    {
        PlayerScoreText.SetActive(false);
        PlayerNameText.SetActive(false);
        ScoreText.SetActive(false);
        WinText.SetActive(false);
        ExitButton.SetActive(false);
        RestartButton.SetActive(false);
        PauseButton.SetActive(false);
    }

    public void OpenPauseMenu()
    {
        switch (Time.timeScale) {
            case 0: ResumeGame(); break;
            case 1: PauseGame(); break;
        }
    }


    public void PauseGame()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }
}
