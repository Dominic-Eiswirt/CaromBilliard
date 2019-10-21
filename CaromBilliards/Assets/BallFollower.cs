using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFollower : MonoBehaviour
{
    public GameObject WhiteBall;
    IPlayerBall whiteBall;    
    public Vector3 offset;    
    
    
    void Start()
    {
        whiteBall = WhiteBall.GetComponent<IPlayerBall>();
    }


    void LateUpdate()
    {
        GoTrack();
    }    
    
    void GoTrack()
    {
        if (!whiteBall.IsBallMoving())
        {
            transform.position = WhiteBall.transform.TransformPoint(offset);
            transform.LookAt(WhiteBall.transform);
        }
    }

    bool IsThereCameraInput()
    {
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            return true;
        }
        return false;
    }
}
