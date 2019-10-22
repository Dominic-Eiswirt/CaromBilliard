using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.SceneManagement;


//Class that contains information about the overall stats of the game related to winning.
public class SessionInfo : MonoBehaviour, ISessionInfo
{
    struct LastTurnBallInformation
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    LastTurnBallInformation whiteBallLastTurn = new LastTurnBallInformation();
    LastTurnBallInformation redBallLastTurn = new LastTurnBallInformation();
    LastTurnBallInformation yellowBallLastTurn = new LastTurnBallInformation();
    float registeredPower;
    float registeredPlayerStrength;
    //References
#pragma warning disable CS0649
    [Header("UI Text")]
    [SerializeField] Text shotsFiredText;
    [SerializeField] Text timerText;
    [SerializeField] Text scoreText;
    [Header("Game Objects")]
    [SerializeField] GameObject PlayerBall;
    IPlayerBall playerBall;
    GameObject yellowBall, redBall;
    [SerializeField] GameObject VictoryTransfer;
#pragma warning restore

    StringBuilder sbTimer = new StringBuilder();
    StringBuilder sbShotsFired = new StringBuilder();
    StringBuilder sbScore = new StringBuilder();
    int gameMoves = 0;
    int score = 0;
    float timer = 1;
    
    public bool IsSimulating { get; private set; }
    bool internalSimulationDelayComplete;
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
        if (!IsSimulating)
        {
            timer += Time.deltaTime;
            //Start at index 6 which is the last index of the string "Timer: ", go until the end of the string to remove the timer numbers
            sbTimer.Remove(6, sbTimer.Length - 6);
            sbTimer.Append(timer.ToString("#"));
            timerText.text = sbTimer.ToString();

            CheckWinCondition();
        }
        else if(internalSimulationDelayComplete)
        {            
            PlayerBall.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z)) * registeredPower * registeredPlayerStrength);
            if(!playerBall.IsBallMoving())
            {
                IsSimulating = false;
                internalSimulationDelayComplete = false;
            }
        }
    }
    public void AddMoveToInput()
    {
        if (!IsSimulating)
        {
            gameMoves++;
            UpdateShotFiredUI();
        }
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
        if (!IsSimulating)
        {
            score++;
            UpdateScoreUI();
        }
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

    public void RegisterBallPositionsEndOfTurn(GameObject ball)
    {
        Ball.BallType ballType = ball.GetComponent<IBall>().GetBallType();
        switch (ballType)
        {
            case Ball.BallType.White:
                whiteBallLastTurn.position = ball.transform.position;
                whiteBallLastTurn.rotation = ball.transform.rotation;
                break;
            case Ball.BallType.Red:
                redBallLastTurn.position = ball.transform.position;
                redBallLastTurn.rotation = ball.transform.rotation;
                redBall = ball;
                break;
            case Ball.BallType.Yellow:
                yellowBallLastTurn.position = ball.transform.position;
                yellowBallLastTurn.rotation = ball.transform.rotation;
                yellowBall = ball;
                break;
        }
    }

    public void RegisterPower(float power, float strength)
    {
        this.registeredPower = power;
        this.registeredPlayerStrength = strength;
    }

    
    public void SimulationButtonPressed()
    {
        if (gameMoves > 0 && !IsSimulating)
        {
            IsSimulating = true;
            yellowBall.transform.position = yellowBallLastTurn.position;
            yellowBall.transform.rotation = yellowBallLastTurn.rotation;
            redBall.transform.position = redBallLastTurn.position;
            redBall.transform.rotation = redBallLastTurn.rotation;
            PlayerBall.transform.position = whiteBallLastTurn.position;
            PlayerBall.transform.rotation = whiteBallLastTurn.rotation;
            StartCoroutine(SimulationDelay());
        }
    }

    IEnumerator SimulationDelay()
    {
        yield return new WaitForSeconds(2f);
        internalSimulationDelayComplete = true;
    }
    
}
