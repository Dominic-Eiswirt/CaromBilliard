using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class that contains information about the overall stats of the game related to winning.
public class SessionInfo : MonoBehaviour, ISessionInfo
{
    int gameMoves = 0;
    int score = 0;
    [SerializeField] Text shotsFiredText;
    [SerializeField] Text timerText;
    [SerializeField] Text scoreText;
    [SerializeField] GameObject PlayerBall;
    IPlayerBall playerBall;
    float timer;

    void OnEnable()
    {
        playerBall.PlayerScoredEvent += AddScore;
    }
    void OnDisable()
    {
        playerBall.PlayerScoredEvent -= AddScore;
    }

    void Awake()
    {
        playerBall = PlayerBall.GetComponent<IPlayerBall>();
        UpdateShotFiredUI();
        UpdateScoreUI();
        
    }

    void Update()
    {
        timer += Time.deltaTime;
        timerText.text = string.Format("Time: {0}", timer.ToString("#"));
    }
    public void AddMoveToInput()
    {
        gameMoves++;
        UpdateShotFiredUI();
    }

    void UpdateShotFiredUI()
    {
        shotsFiredText.text = string.Format("Shots Fired: {0}", gameMoves.ToString());
    }

    public void AddScore()
    {
        score++;
        UpdateScoreUI();
    }
    void UpdateScoreUI()
    {
        scoreText.text = string.Format("Score: {0}", score.ToString());
    }
}
