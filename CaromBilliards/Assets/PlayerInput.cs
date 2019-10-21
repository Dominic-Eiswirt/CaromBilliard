using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerInput : MonoBehaviour
{

    float pressedDuration = 0f;
    float strength = 200f;
    [SerializeField] GameObject WhiteBall;
    [SerializeField] GameObject Cue;
    [SerializeField] Slider strengthSlider;
    IPlayerBall whiteBall;
    ICue cue;
    void Start()
    {
        whiteBall = WhiteBall.GetComponent<IPlayerBall>();
        cue = Cue.GetComponent<ICue>();
    }
    void Update()
    {
        if (!whiteBall.IsBallMoving())
        {
            if (Input.GetKey(KeyCode.Space))
            {
                pressedDuration += Time.deltaTime * 2;
                cue.AdjustCue(pressedDuration / 100);
                if(pressedDuration > 10)
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

    void Shoot()
    {
        Vector3 cameraDirection = Vector3.Normalize(new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z));
        //whiteBall.ApplyForce(Camera.main.transform.forward * pressedDuration);        
        whiteBall.ApplyForce(cameraDirection * pressedDuration * strength);
        cue.Release();
        pressedDuration = 0;
    }
}
