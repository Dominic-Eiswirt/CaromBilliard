using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFollower : MonoBehaviour
{
    public GameObject WhiteBall;    
    public Vector3 offset;    
    bool tracking = true;
    float dampening = 2f;
    float targetAngle;
    Vector3 targetPosition;
    Quaternion rot;
   
    void LateUpdate()
    {
        GoTrack();
    }
    void GoTrack()
    {
        if(tracking)
        {
            targetPosition = WhiteBall.transform.position - offset;
            targetAngle = WhiteBall.transform.eulerAngles.y;
            rot = Quaternion.Euler(0, Mathf.Abs(targetAngle), 0);
            transform.position = Vector3.Lerp(transform.position, WhiteBall.transform.position - (rot * offset), Time.deltaTime*dampening) ;            
            transform.LookAt(WhiteBall.transform.position);            
        }
    }
}
