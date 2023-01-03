using BumperBallGame;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{

    [Tooltip("Win game message")]
    public string WinGameMessage;

    [Tooltip("Duration of delay before the win message")]
    public float DelayBeforeWinMessage = 2f;

    [Tooltip("Restart button")]
    public GameObject RestartButton;
    [Tooltip("Exit button")]
    public GameObject ExitButton;
    [Tooltip("Win Text UI")]
    public GameObject WinText;
    public GameObject ScoreText;
    public GameModeManager GameModeManager;

    void Awake()
    {
        EventManager.AddListener<PlayerOutOfLivesEvent>(OnPlayerOutOfLives);
        EventManager.AddListener<GameOverEvent>(OnGameOver);
        EventManager.AddListener<PlayerDeathEvent>(OnPlayerDeath);
        GameModeManager = GetComponent<GameModeManager>();
        string scores = "Scores\n\n";
        foreach (GameObject p in GameModeManager.players)
        {
            scores += p.transform.parent.name + " " + GameModeManager.scores[p] + "\n";
        }
        ScoreText.GetComponent<TMP_Text>().SetText(scores);
    }


    void OnPlayerOutOfLives(PlayerOutOfLivesEvent evt)
    {
        if (evt.Killed.GetComponent<PlayerController>())
        {
            RestartButton.SetActive(true);
            ExitButton.SetActive(true);
        }
    }

    void OnGameOver(GameOverEvent evt)
    {
        WinText.GetComponent<TMP_Text>().SetText(evt.Winner.transform.parent.name + " " + WinGameMessage);
        RestartButton.SetActive(true);
        ExitButton.SetActive(true);
        WinText.SetActive(true);
    }

    void OnPlayerDeath(PlayerDeathEvent evt)
    {
        string scores = "Scores\n\n";
        foreach (GameObject p in GameModeManager.players)
        {
            scores += p.transform.parent.name + " " + GameModeManager.scores[p] + "\n";
        }
        ScoreText.GetComponent<TMP_Text>().SetText(scores);
    }

    void OnDestroy()
    {
        EventManager.RemoveListener<PlayerOutOfLivesEvent>(OnPlayerOutOfLives);
        EventManager.RemoveListener<GameOverEvent>(OnGameOver);
        EventManager.RemoveListener<PlayerDeathEvent>(OnPlayerDeath);
    }
}
