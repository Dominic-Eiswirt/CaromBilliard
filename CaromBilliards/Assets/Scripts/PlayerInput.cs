using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInput : MonoBehaviour
{
    float pressedDuration = 0f;
    float strength = 200f;    
    [SerializeField] GameObject WhiteBall;
    IPlayerBall whiteBall;
    [SerializeField] GameObject Cue;
    ICue cue;
    ISessionInfo sessionInfo;
    [SerializeField] Slider strengthSlider;

    void Start()
    {
        whiteBall = WhiteBall.GetComponent<IPlayerBall>();
        cue = Cue.GetComponent<ICue>();
        sessionInfo = this.GetComponent<ISessionInfo>();
    }

    void Update()
    {
        //Wait until ball is stationary
        if (!whiteBall.IsBallMoving())
        {
            if (Input.GetKey(KeyCode.Space))
            {
                pressedDuration += Time.deltaTime * 2;
                cue.AdjustCue(pressedDuration / 100);
                //Release shot if the duration is at maximum of our slider value
                if (pressedDuration > strengthSlider.maxValue)
                {
                    Shoot();
                }
            }
            else if (pressedDuration > 0)
            {
                Shoot();
            }
        }
        strengthSlider.value = pressedDuration;
    }

    //Shoot the ball, reset the pull, and add moves to the game score
    void Shoot()
    {
        Vector3 cameraDirection = Vector3.Normalize(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z));        
        whiteBall.ApplyForce(cameraDirection * pressedDuration * strength);
        cue.Release();
        pressedDuration = 0;
        sessionInfo.AddMoveToInput();
    }
}
