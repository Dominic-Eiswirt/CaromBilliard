using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that follows the ball and changes the cue's position when it gets told to do so by playerinput and sessioninfo (replay simulation)
public class Cue : BallFollower, ICue
{        
    bool firing;
    float strengthValue;
    float distanceModifier = 10f;

    void Start()
    {
        whiteBall = WhiteBall.GetComponent<IPlayerBall>();
    }

    void LateUpdate()
    {
        if (!firing)
        {
            GoTrack();
        }

    }

    public void AdjustCue(float value)
    {
        //Draw back cue based on time pressed * backward
        value = value / 120f;
        firing = true;
        transform.position = WhiteBall.transform.TransformPoint(offset) + (value * -transform.forward * distanceModifier);
        transform.LookAt(WhiteBall.transform);
    }

    public void Release()
    {
        //Set bool off so we can track in update()
        transform.position = WhiteBall.transform.TransformPoint(offset);
        transform.LookAt(WhiteBall.transform);
        firing = false;
    }
}
