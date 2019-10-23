using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PlayerInput : MonoBehaviour, IPlayerInput
{    
#pragma warning disable CS0649
    [SerializeField] GameObject WhiteBall, RedBall, YellowBall;
    IPlayerBall whiteBall;
    [SerializeField] GameObject Cue;
    ICue cue;
    ISessionInfo sessionInfo;
    [SerializeField] Slider strengthSlider;
    [SerializeField] GameObject ReplayUI;    
#pragma warning restore

    float pressedDuration = 0f;
    float strength = 100f;    

    void Start()
    {
        whiteBall = WhiteBall.GetComponent<IPlayerBall>();
        cue = Cue.GetComponent<ICue>();
        sessionInfo = this.GetComponent<ISessionInfo>();
    }

    void Update()
    {
        //Wait until ball is stationary
        if (!whiteBall.IsBallMoving() && !sessionInfo.IsSimulating)
        {
            if(Input.GetKeyUp(KeyCode.Space))
            {
                Shoot();
            }
            if (Input.GetKey(KeyCode.Space))
            {
                pressedDuration += Time.deltaTime * 2;
                cue.AdjustCue(pressedDuration);

                //Release shot if the duration is at maximum of our slider value
                if (pressedDuration > strengthSlider.maxValue)
                {
                    Shoot();
                }               
            }
            else //Don't rotate camera while shooting
            {
                if (Input.GetKey(KeyCode.A))
                {
                    WhiteBall.transform.Rotate(0, -Time.deltaTime * 75f, 0, Space.Self);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    WhiteBall.transform.Rotate(0, Time.deltaTime * 75f, 0, Space.Self);
                }
            }
            strengthSlider.value = pressedDuration;
        }

        ShowUIIfAble();
    }
    
    //Shoot the ball, register what is necessary, and reset the cue
    public void Shoot()
    {
        sessionInfo.AddMoveToInput();
        sessionInfo.RegisterBallPositionsEndOfTurn(WhiteBall);
        sessionInfo.RegisterBallPositionsEndOfTurn(RedBall);
        sessionInfo.RegisterBallPositionsEndOfTurn(YellowBall);
        sessionInfo.RegisterPower(pressedDuration, strength);        
        Vector3 cameraDirection = Vector3.Normalize(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z));        
        whiteBall.ApplyForce(cameraDirection * pressedDuration * strength);
        cue.Release();
        pressedDuration = 0;
    }

    void ShowUIIfAble()
    {
        if (!whiteBall.IsBallMoving())
        {
            ReplayUI.SetActive(true);            
        }
        else
        {
            ReplayUI.SetActive(false);            
        }
    }

    public void SetStrengthSliderForSimulation(float value)
    {
        strengthSlider.value = value;
        //Not ideal to jam the duration in here. But basically it needs to be set at 0 during simulation
        //because otherwise it will sometimes shoot the ball at the end of the turn
        //on its own because presseduration > slider value
        pressedDuration = 0;
    }
}
