using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.SceneManagement;
//Class that contains information about the overall stats of the game related to winning.
public class SessionInfo : MonoBehaviour, ISessionInfo
{
//References
#pragma warning disable CS0649
    [Header("UI Text")]
    [SerializeField] Text shotsFiredText;
    [SerializeField] Text timerText;
    [SerializeField] Text scoreText;    
    [Header("Game Objects")]
    [SerializeField] GameObject PlayerBall;
    IPlayerBall playerBall;
    [SerializeField] GameObject VictoryTransfer;
#pragma warning restore
    StringBuilder sbTimer = new StringBuilder();
    StringBuilder sbShotsFired = new StringBuilder();
    StringBuilder sbScore = new StringBuilder();
    int gameMoves = 0;
    int score = 0;
    float timer=1;

   
    void Awake()
    {       

        UpdateShotFiredUI();
        UpdateScoreUI();
        
        sbTimer.Insert(0, "Time: ");
        sbTimer.Append(timer.ToString("#"));

        playerBall = PlayerBall.GetComponent<IPlayerBall>();
    }

    //Events for when the ball scores. Let the ball handle calculation, and use delegate to trigger when we score
    void OnEnable()
    {
        playerBall.PlayerScoredEvent += AddScore;
    }

    void OnDisable()
    {
        playerBall.PlayerScoredEvent -= AddScore;
    }


    void Update()
    {
        timer += Time.deltaTime;        
        //Start at index 6 which is the last index of the string "Timer: ", go until the end of the string to remove the timer numbers
        sbTimer.Remove(6, sbTimer.Length-6);
        sbTimer.Append(timer.ToString("#"));        
        timerText.text = sbTimer.ToString();

        CheckWinCondition();
    }
    public void AddMoveToInput()
    {
        gameMoves++;
        UpdateShotFiredUI();
    }

    void UpdateShotFiredUI()
    {
        sbShotsFired.Insert(0, "Shots Fired: ");
        sbShotsFired.Remove(13, sbShotsFired.Length - 13);
        sbShotsFired.Append(gameMoves.ToString());
        shotsFiredText.text = sbShotsFired.ToString();        
    }

    public void AddScore()
    {
        score++;
        UpdateScoreUI();        
    }

    void CheckWinCondition()
    {
        //If score is at 3 and the ball is not moving we load new scene. Waiting for ball movement so that it doesn't load new scene
        // the moment a collision happens 
        if (score >= 3 && !playerBall.IsBallMoving())
        {
            VictoryTransfer.GetComponent<IVictoryTransfer>().SetSessionInformation(score, gameMoves, timer);
            SceneManager.LoadScene(2);
        }
    }

    void UpdateScoreUI()
    {
        sbScore.Insert(0, "Score: ");
        sbScore.Remove(7, sbScore.Length - 7);
        sbScore.Append(score.ToString());
        scoreText.text = sbScore.ToString();        
    }
}
