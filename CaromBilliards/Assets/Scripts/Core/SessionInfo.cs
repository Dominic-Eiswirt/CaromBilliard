using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.SceneManagement;


//Class that contains information about the overall stats of the current game.
public class SessionInfo : MonoBehaviour, ISessionInfo
{

    //References set in inspector
#pragma warning disable CS0649    
    [Header("UI Text")]
    [SerializeField] Text shotsFiredText;
    [SerializeField] Text timerText;
    [SerializeField] Text scoreText;
    [Header("Game Objects")]
    [SerializeField] GameObject WhiteBall;
    GameObject yellowBall, redBall;
    [SerializeField] GameObject VictoryTransfer;
    [SerializeField] GameObject Cue;
#pragma warning restore

    IPlayerBall whiteBall;
    ICue cue;
    IPlayerInput playerInput;
    bool scored;

    //Simulation control to determine if we are replaying or not
    //Internal Simulation refers to the cue stick and the drawing with UI. The "core" simulation is the actual balls moving
    public bool IsSimulating { get; private set; }
    bool internalSimulationDelayComplete;

    //For storing information about the finished turn.
    struct LastTurnBallInformation
    {
        public Vector3 position;
        public Quaternion rotation;
    }
    LastTurnBallInformation whiteBallLastTurn = new LastTurnBallInformation();
    LastTurnBallInformation redBallLastTurn = new LastTurnBallInformation();
    LastTurnBallInformation yellowBallLastTurn = new LastTurnBallInformation();    
    float registeredPower;
    float simulationPower;
    float registeredPlayerStrength;
    int gameMoves = 0;
    int score = 0;
    float timer = 1;
    
    //String builder to avoid issues with string concatenation
    StringBuilder sbTimer = new StringBuilder();
    StringBuilder sbShotsFired = new StringBuilder();
    StringBuilder sbScore = new StringBuilder();


    void Awake()
    {    
        UpdateShotFiredUI();
        UpdateScoreUI();
        
        sbTimer.Insert(0, "Time: ");
        sbTimer.Append(timer.ToString("#"));

        playerInput = GetComponent<IPlayerInput>();
        whiteBall = WhiteBall.GetComponent<IPlayerBall>();
        cue = Cue.GetComponent<ICue>();
    }

    //Events for when the ball scores. Let the ball handle calculation, and use delegate to trigger when we score
    void OnEnable()
    {
        whiteBall.PlayerScoredEvent += AddScore;
    }
    void OnDisable()
    {
        whiteBall.PlayerScoredEvent -= AddScore;
    }


    void Update()
    {        
        if (!IsSimulating)
        {   
            //Reset the scored bool if we have a new turn. Not in simulation and ball != moving means it's a new turn
            if(!whiteBall.IsBallMoving())
            {
                scored = false;
            }
            timer += Time.deltaTime;
            //Start at index 6 which is the last index of the string "Timer: ", go until the end of the string to remove the timer numbers
            sbTimer.Remove(6, sbTimer.Length - 6);
            sbTimer.Append(timer.ToString("#"));
            timerText.text = sbTimer.ToString();
            
            CheckWinCondition();
        }
        else if(internalSimulationDelayComplete)
        {            
                
            if(!whiteBall.IsBallMoving())
            {
                IsSimulating = false;
                internalSimulationDelayComplete = false;
                cue.Release();
            }
        }
        else
        {
            simulationPower += Time.deltaTime * 2;
            cue.AdjustCue(simulationPower);
            playerInput.SetStrengthSliderForSimulation(simulationPower);
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
        if (!IsSimulating && !scored)
        {
            scored = true;
            score++;
            UpdateScoreUI();
        }
    }

    void CheckWinCondition()
    {
        //If score is at 3 and the ball is not moving we load new scene. Waiting for ball movement so that it doesn't load new scene
        // the moment a collision happens 
        if (score >= 3 && !whiteBall.IsBallMoving())
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

    //Function that copies information from the ball. Should be called before the white ball is fired.
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
            simulationPower = 0;
            yellowBall.transform.position = yellowBallLastTurn.position;
            yellowBall.transform.rotation = yellowBallLastTurn.rotation;
            redBall.transform.position = redBallLastTurn.position;
            redBall.transform.rotation = redBallLastTurn.rotation;
            WhiteBall.transform.position = whiteBallLastTurn.position;
            WhiteBall.transform.rotation = whiteBallLastTurn.rotation;
            cue.AdjustCue(0);
            StartCoroutine(SimulationDelayControl());
        }
    }

    IEnumerator SimulationDelayControl()
    {        
        //Use registered power here divided by two (registeredPower is time pressed * 2 in PlayerInput)        
        yield return new WaitForSeconds(registeredPower / 2);
        WhiteBall.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(
                                                                   new Vector3(
                                                                       Camera.main.transform.forward.x, //x
                                                                       0, //y
                                                                       Camera.main.transform.forward.z)) * //z
                                                                       registeredPower *
                                                                       registeredPlayerStrength);
        cue.Release();

        //We must wait until Unity registers the ball as moving. Failure to do so means that the timer will continue incrementing while
        //the ball moves in the simulation (the simulation bools will be reset immediately 
        //because the IsBallMoving() check will return false for one frame).  
        while(!whiteBall.IsBallMoving())
        {
            yield return null;
        }
        internalSimulationDelayComplete = true;        
    }
    
}
