using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    float pressedDuration = 0f;    
    public GameObject WhiteBall;
    public GameObject Cue;
    IBall whiteBall;
    ICue cue;
    void Start()
    {
        whiteBall = WhiteBall.GetComponent<IBall>();
        cue = Cue.GetComponent<ICue>();
    }
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            pressedDuration += Time.deltaTime * 100f;
            cue.AdjustCue(pressedDuration/100);
        }
        else if(pressedDuration > 0)
        {
            whiteBall.ApplyForce((WhiteBall.transform.position + transform.forward).normalized * pressedDuration);
            cue.Release();
            pressedDuration = 0;
        }
    }
}
